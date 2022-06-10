package com.example.amqp;

import com.rabbitmq.client.Channel;
import com.rabbitmq.client.Connection;
import com.rabbitmq.client.ConnectionFactory;
import com.rabbitmq.client.DeliverCallback;
import org.apache.commons.codec.digest.DigestUtils;
import java.nio.charset.StandardCharsets;
import java.util.concurrent.CountDownLatch;

/** RabbitMQDemo */

public class App {

    public static void main(String[] args) {

        String accessKey = "24b9rucZxRMcoar7iyjCEWGF";

        String accessSecret = "69bHCUXuFYkxcwDemgD9NnsextyhCeMFYZ81PBjS";

        String url = "amqps://iot-amqp.quectelcn.com:5671/quec-open", queueName = "a.000000007550.test_queue";

        long timestamp = System.currentTimeMillis();

        String username = String.format("ver=1&auth_mode=accessKey&sign_method=sha256&access_key=%s&timestamp=%s",
                accessKey, timestamp);

        String password = DigestUtils.sha256Hex(username + accessSecret).toLowerCase();

        try {

            CountDownLatch latch = new CountDownLatch(1);

            ConnectionFactory factory = new ConnectionFactory();

            factory.setUri(url);

            factory.setUsername(username);

            factory.setPassword(password);

//            System.out.println(url);
//            System.out.println(username);
//            System.out.println(password);

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
