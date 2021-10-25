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
	c.String(200, "get topicId=%s post", c.Param("topic_id"))
}

func NewTopic(c *gin.Context) {
	c.String(200, "New topic")
}

func DelTopic(c *gin.Context) {
	c.String(200, "Delete topic")
}
