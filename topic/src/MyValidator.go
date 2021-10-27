package src

import (
	"github.com/go-playground/validator/v10"
	"reflect"
	"regexp"
)

func TopicUrl(v *validator.Validate, topStruct reflect.Value, currentStructOrField reflect.Value, field reflect.Value, fieldType reflect.Type, fieldKind reflect.Kind, param string) bool {
	if _, ok := topStruct.Interface().(*Topic); ok {
		if matched, _ := regexp.MatchString(`^\w{4,10}$`, field.String()); matched {
			return true
		}
	}
	return false
}
