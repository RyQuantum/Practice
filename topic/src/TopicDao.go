package src

import (
	"github.com/gin-gonic/gin"
	"net/http"
)

func MustLogin() gin.HandlerFunc {
	return func(c *gin.Context) {
		if _, status := c.GetQuery("token"); !status {
			c.String(http.StatusUnauthorized, "cannot find token")
			c.Abort()
		} else {
			c.Next()
		}
	}
}

func GetTopicDetail(c *gin.Context) {
	c.JSON(200, CreateTopic(101, "post title"))
}

func NewTopic(c *gin.Context) {
	topic := Topic{}
	err := c.BindJSON(&topic)
	if err != nil {
		c.String(400, "invalid parameter: %s", err.Error())
	} else {
		c.JSON(200, topic)
	}
}

func NewTopics(c *gin.Context) {
	topics := Topics{}
	err := c.BindJSON(&topics)
	if err != nil {
		c.String(400, "invalid parameter: %s", err.Error())
	} else {
		c.JSON(200, topics)
	}
}

func DelTopic(c *gin.Context) {
	c.String(200, "Delete topic")
}

func GetTopicList(c *gin.Context) {
	query := TopicQuery{}
	err := c.BindQuery(&query)
	if err != nil {
		c.String(400, "invalid parameter: %s", err.Error())
	} else {
		c.JSON(200, query)
	}
}
