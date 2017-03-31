package main

import (
	"fmt"
	"os"
	"ejb1123.tk/FiveMBuilder/cmd"
)

func main() {
	if err := cmd.RootCmd.Execute(); err != nil {
		fmt.Println(err)
		os.Exit(-1)
	}
}
