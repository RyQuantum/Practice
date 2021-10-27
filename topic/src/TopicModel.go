package src

type Topic struct {
	TopicID         int    `json:"id"`
	TopicTitle      string `json:"title" binding:"min=4,max=20"`
	TopicShortTitle string `json:"stitle" binding:"required,nefield=TopicTitle"`
	UserIP          string `json:"ip" binding:"ipv4"`
	TopicScore      int    `json:"score" binding:"omitempty,gt=5"`
	TopicUrl        string `json:"url" binding:"omitempty,topicurl"`
}

type Topics struct {
	TopicList     []Topic `json:"topics" binding:"gt=0,lte=3,dive"`
	TopicListSize int     `json:"size"` //TODO size validation
}

func CreateTopic(id int, title string) Topic {
	return Topic{TopicID: id, TopicTitle: title}
}

type TopicQuery struct {
	UserName string `json:"username" form:"username"`
	Page     int    `json:"page" form:"page" binding:"required"`
	PageSize int    `json:"pagesize" form:"pagesize"`
}
