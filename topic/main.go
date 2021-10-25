package main

import (
	"github.com/gin-gonic/gin"
	. "topic/src"
)

func main() {
	router := gin.Default()
	v1 := router.Group("/v1/topics")
	{
		v1.GET("", func(c *gin.Context) {
			if c.Query("username") == "" {
				c.String(200, "get topic list")
			} else {
				c.String(200, "get username=%s topic list", c.Query("username"))
			}
		})

		v1.GET("/:topic_id", GetTopicDetail)

		v1.Use(MustLogin())
		{
			v1.POST("", NewTopic)
			v1.DELETE("/:topic_id", DelTopic)
		}
	}

	router.Run()
}
