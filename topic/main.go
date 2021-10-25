package main

import (
	"github.com/gin-gonic/gin"
)

func main() {
	router := gin.Default()

	router.GET("/v1/topics", func(c *gin.Context) {
		if c.Query("username") == "" {
			c.String(200, "get topic list")
		} else {
			c.String(200, "get username=%s topic list", c.Query("username"))
		}
	})

	router.GET("/v1/topics/:topic_id", func(c *gin.Context) {
		c.String(200, "get topicId=%s topic", c.Param("topic_id"))
	})

	router.Run()
}
