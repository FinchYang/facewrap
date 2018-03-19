package main

import(
	"net/http"
)

func main(){
	http.ListenAndServe(":888",http.FileServer(http.Dir(`G:\doc`)))
}