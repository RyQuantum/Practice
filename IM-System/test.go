package main

import (
	"fmt"
)

type Person struct {
	Name string
	Sex  bool
}

func main() {
	p := Person{"Ryan", true}
	v := &p
	println(v)
	println("1", p.Name, p.Sex)
	changeName(p)
	fmt.Println("2", p.Name, p.Sex)
}

func changeName(p Person) {
	println(&p)
	p.Name = "Rainie"
}
