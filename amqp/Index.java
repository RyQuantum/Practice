package amqp;

import com.rabbitmq.client.Channel;
import com.rabbitmq.client.Connection;
import com.rabbitmq.client.ConnectionFactory;
import com.rabbitmq.client.DeliverCallback;
import org.apache.commons.codec.digest.DigestUtils;
import java.nio.charset.StandardCharsets;
import java.util.concurrent.CountDownLatch;

/** RabbitMQDemo */

public class Index {

    public static void main(String[] args) {

        String accessKey = "${accessKey}";

        String accessSecret = "${accessSecret}";

        String url = "${connectionUrl}", queueName = "${queueName}";

        long timestamp = System.currentTimeMillis();

        String username = String.format("ver=1&auth_mode=accessKey&sign_method=sha256&access_key=%s &timestamp=%s",
                accessKey, timestamp);

        String password = DigestUtils.sha256Hex(username + accessSecret).toLowerCase();

        try {

            CountDownLatch latch = new CountDownLatch(1);

            ConnectionFactory factory = new ConnectionFactory();

            factory.setUri(url);

            factory.setUsername(username);

            factory.setPassword(password);

            factory.useSslProtocol();

            try (Connection conn = factory.newConnection(); Channel channel = conn.createChannel()) {

                channel.basicQos(1); // DeliverCallback

                DeliverCallback deliverCallback = (consumerTag, delivery) -> {

                    try {
                        String message = new String(delivery.getBody(), StandardCharsets.UTF_8); // handle

                        System.out.println(
                                "Received message: '" + message + "', timestamp: " + System.currentTimeMillis());

                    } finally {

                        channel.basicAck(delivery.getEnvelope().getDeliveryTag(), false);
                    }

                }; // acutoAck true/false

                channel.basicConsume(queueName, false, deliverCallback, consumerTag -> {

                    System.out.println("The consumer is cancelled");

                    latch.countDown();
                });

                latch.await();

            }

        } catch (Exception e) {

            e.printStackTrace();

        }
    }
}