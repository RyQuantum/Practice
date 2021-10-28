package main

import (
	"fmt"
	"github.com/gin-gonic/gin"
	"github.com/gin-gonic/gin/binding"
	"github.com/go-playground/validator/v10"
	"gorm.io/driver/mysql"
	"gorm.io/gorm"
	"io/ioutil"
	"net/http"
	. "topic/src"
)

func connectDb() {
	dsn := "ryan:password@tcp(192.168.2.9:3306)/hkzf?charset=utf8&parseTime=True&loc=Local"
	db, _ := gorm.Open(mysql.Open(dsn), &gorm.Config{})
	sqlDB, _ := db.DB()
	defer sqlDB.Close()
	rows, _ := db.Raw("select id, name, code from characteristics").Rows()
	for rows.Next() {
		var c_id int
		var c_name string
		var c_code string
		rows.Scan(&c_id, &c_name, &c_code)
		fmt.Println(c_id, c_name, c_code)
	}
}

func main() {
	//connectDb()
	router := gin.Default()
	if v, ok := binding.Validator.Engine().(*validator.Validate); ok {
		v.RegisterValidation("topicurl", TopicUrl)
		//v.RegisterValidation("topics", TopicsValidate)
	}

	router.POST("", func(c *gin.Context) {
		file, header, err := c.Request.FormFile("client.csr")
		if err != nil {
			c.JSON(http.StatusBadRequest, gin.H{"msg": "文件上传失败"})
			return
		}

		con, err := ioutil.ReadAll(file)
		if err != nil {
			c.JSON(http.StatusBadRequest, gin.H{"msg": "文件读取失败"})
			return
		}

		fmt.Println(header.Filename)
		fmt.Println(string(con))
		//c.JSON(http.StatusOK, gin.H{"msg": "上传成功"})
		content := c.Query("content")

		content = "-----BEGIN CERTIFICATE-----\nMIIB4TCCAYcCFG0AkBIQKdDKL82+ofUY9KF8SsSAMAoGCCqGSM49BAMCMHMxCzAJ\nBgNVBAYTAkNOMQswCQYDVQQIDAJCSjELMAkGA1UEBwwCQkoxCzAJBgNVBAoMAkhE\nMQwwCgYDVQQLDANvcHMxDzANBgNVBAMMBlBDVG9vbDEeMBwGCSqGSIb3DQEJARYP\ncnlhbkByZW50bHkuY29tMB4XDTIxMDkxODAzMzg1MFoXDTIyMDkxODAzMzg1MFow\nczELMAkGA1UEBhMCQ04xCzAJBgNVBAgMAkJKMQswCQYDVQQHDAJCSjELMAkGA1UE\nCgwCSEQxDDAKBgNVBAsMA29wczEPMA0GA1UEAwwGUENUb29sMR4wHAYJKoZIhvcN\nAQkBFg9yeWFuQHJlbnRseS5jb20wWTATBgcqhkjOPQIBBggqhkjOPQMBBwNCAAS2\n+RkBO64NQbjW37ZWPQdpAgKiYiKf7z7GkMfjRBXuVwPYmYE9f9Kd36ZAEkqe1KoF\n9diRYWZ1ZZIru/1EmYg+MAoGCCqGSM49BAMCA0gAMEUCIHP5LaQBeCiMrsepSelQ\nfRNENaLzXwpo6P6j2xU19HSTAiEA/qDt/3Sc8NPaNd1Tgx3XfHEv93yeD19/NE8Z\nDZkaCJY=\n-----END CERTIFICATE-----\n" + content

		c.Writer.WriteHeader(http.StatusOK)
		c.Header("Content-Disposition", "attachment; filename=hello.txt")
		c.Header("Content-Type", "application/text/plain")
		c.Header("Accept-Length", fmt.Sprintf("%d", len(content)))
		c.Writer.Write([]byte(content))
	})

	v1 := router.Group("/v1/topics")
	{
		v1.GET("", GetTopicList)

		v1.GET("/:topic_id", GetTopicDetail)

		v1.Use(MustLogin())
		{
			v1.POST("", NewTopic)
			v1.DELETE("/:topic_id", DelTopic)
		}
	}

	v2 := router.Group("/v1/mtopics")
	{
		v2.Use(MustLogin())
		{
			v2.POST("", NewTopics)
		}
	}

	router.Run()
}
