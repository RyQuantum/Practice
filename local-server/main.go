package main

import "C"
import (
	"fmt"
	"github.com/gin-gonic/gin"
	"github.com/grandcat/zeroconf"
	"gorm.io/driver/sqlite"
	"gorm.io/gorm"
	"log"
	"net"
)

var db *gorm.DB

//export Start
func Start() {
	var err error
	db, err = gorm.Open(sqlite.Open("sqlite.db"), &gorm.Config{})
	fmt.Println("err:", err)
	fmt.Println("db:", db)

	setupMdns()
	router := gin.Default()
	v1 := router.Group("/")
	{
		v1.GET("/", helloWorld)
		v1.POST("/lock", createLock)
	}
	router.Run("0.0.0.0:25348")
}
func main() {
	//var err error
	//db, err = gorm.Open(sqlite.Open("sqlite.db"), &gorm.Config{})
	//fmt.Println("err:", err)
	//fmt.Println("db:", db)
	//
	//setupMdns()
	router := gin.Default()
	v1 := router.Group("/")
	{
		v1.GET("/", helloWorld)
		v1.POST("/lock", createLock)
	}
	router.Run("0.0.0.0:25348")
}

func helloWorld(c *gin.Context) {
	c.JSON(200, gin.H{"message": "Connected", "success": true})
}

func GetOutboundIP() net.IP {
	conn, err := net.Dial("udp", "8.8.8.8:80")
	if err != nil {
		log.Fatal(err)
	}
	defer conn.Close()

	localAddr := conn.LocalAddr().(*net.UDPAddr)

	return localAddr.IP
}

func setupMdns() {
	ip := GetOutboundIP()
	_, err := zeroconf.Register("Ryan's MacBook Air", "_http._tcp.", "local.", 80, []string{"ip=" + ip.String()}, nil)
	if err != nil {
		panic(err)
	}
}

func createLock(c *gin.Context) {

	var lock Locks

	if err := c.BindJSON(&lock); err != nil {
		// DO SOMETHING WITH THE ERROR
	}

	db.Create(&lock)
	c.JSON(200, gin.H{"message": lock.SerialNumber + " saved", "success": true})
}

type Locks struct {
	SerialNumber string `json:"serial_number"`
}
