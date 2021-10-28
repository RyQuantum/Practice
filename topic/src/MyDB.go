package src

import (
	"fmt"
	"gorm.io/driver/mysql"
	"gorm.io/gorm"
	"gorm.io/gorm/logger"
)

var DBHealper *gorm.DB
var err error

func init() {
	dsn := "ryan:password@tcp(192.168.2.9:3306)/hkzf?charset=utf8&parseTime=True&loc=Local"
	DBHealper, err = gorm.Open(mysql.Open(dsn), &gorm.Config{
		Logger: logger.Default.LogMode(logger.Error),
	})
	if err != nil {
		fmt.Println(err)
	}
}
