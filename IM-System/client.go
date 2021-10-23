package main

import (
	"bufio"
	"flag"
	"fmt"
	"io"
	"net"
	"os"
)

type Client struct {
	ServerIp   string
	ServerPort int
	Name       string
	conn       net.Conn
	flag       int
	scanner    *bufio.Scanner
}

func NewClient(serverIp string, serverPort int) *Client {
	client := &Client{
		ServerIp:   serverIp,
		ServerPort: serverPort,
		flag:       999,
		scanner:    bufio.NewScanner(os.Stdin),
	}

	conn, err := net.Dial("tcp", fmt.Sprintf("%s:%d", serverIp, serverPort))
	if err != nil {
		fmt.Printf("net.Dial error:", err)
		return nil
	}

	client.conn = conn
	return client
}

func (Client *Client) DealResponse() {
	io.Copy(os.Stdout, Client.conn)
}

func (client *Client) menu() bool {
	var flag int
	fmt.Println("1. Public mode")
	fmt.Println("2. Private mode")
	fmt.Println("3. Update username")
	fmt.Println("0. Exit")

	fmt.Scanln(&flag)

	if flag >= 0 && flag <= 3 {
		client.flag = flag
		return true
	} else {
		fmt.Println(">>>> Please input valid number <<<<")
		return false
	}
}

func (client *Client) ScanInput(msg string) string {
	fmt.Println(msg)
	client.scanner.Scan()
	chatMsg := client.scanner.Text()
	return chatMsg
}

func (client *Client) PublicChat() {
	reqMsg := ">>>> Please input the message you want to send. Input \"exit\" can exit."
	var chatMsg string

	chatMsg = client.ScanInput(reqMsg)

	for chatMsg != "exit" {
		if len(chatMsg) != 0 {
			sendMsg := chatMsg + "\n"
			_, err := client.conn.Write([]byte(sendMsg))
			if err != nil {
				fmt.Println("conn Write err:", err)
				break
			}
		}
		chatMsg = client.ScanInput(reqMsg)
	}
}

func (client *Client) SelectUsers() {
	sendMsg := "who\n"
	_, err := client.conn.Write([]byte(sendMsg))
	if err != nil {
		fmt.Println("conn Write err:", err)
		return
	}
}

func (client *Client) PrivateChat() {
	reqFriend := ">>>> Please input your friend's name. Input \"exit\" can exit."
	reqMsg := ">>>> Please input the message you want to send. Input \"exit\" can exit."
	var remoteName string
	var chatMsg string

	client.SelectUsers()

	remoteName = client.ScanInput(reqFriend)

	for remoteName != "exit" {
		chatMsg = client.ScanInput(reqMsg)

		for chatMsg != "exit" {
			if len(chatMsg) != 0 {
				sendMsg := "to|" + remoteName + "|" + chatMsg + "\n"
				_, err := client.conn.Write([]byte(sendMsg))
				if err != nil {
					fmt.Println("conn Write err:", err)
					break
				}
			}

			chatMsg = client.ScanInput(reqMsg)
		}

		client.SelectUsers()
		remoteName = client.ScanInput(reqFriend)
	}
}

func (client *Client) UpdateName() bool {
	fmt.Println(">>>> Please input the username:")
	fmt.Scanln(&client.Name)

	sendMsg := "rename|" + client.Name + "\n"
	_, err := client.conn.Write([]byte(sendMsg))
	if err != nil {
		fmt.Println("conn.Write err:", err)
		return false
	}
	return true
}

func (client *Client) Run() {
	for client.flag != 0 {
		for client.menu() != true {
		}

		switch client.flag {
		case 1:
			client.PublicChat()
		case 2:
			client.PrivateChat()
		case 3:
			client.UpdateName()
		}
	}
}

var serverIp string
var serverPort int

func init() {
	flag.StringVar(&serverIp, "ip", "incognito.tpddns.cn", "Set the server IP")
	flag.IntVar(&serverPort, "p", 8888, "Set the port")
}

func main() {

	flag.Parse()

	client := NewClient(serverIp, serverPort)
	if client == nil {
		fmt.Println(">>>>> connect to server failed...")
		return
	}

	go client.DealResponse()

	fmt.Println(">>>>> connect to server successfully...")
	client.Run()
}
