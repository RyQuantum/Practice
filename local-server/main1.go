package main

import "C"
import "fmt"

//export Hello
func Hello() {
	fmt.Println("hello world from go dll!")
}
func main() {
}
