package main

import (
	"fmt"
)

func main() {
	var anything interface{} = 34535
	aString, ok := anything.(string)
	if ok {
		fmt.Println(aString)
	} else {
		fmt.Printf(" anything is not string")
	}
}
