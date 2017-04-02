// Copyright © 2017 NAME HERE <EMAIL ADDRESS>
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

package cmd

import (
	"fmt"
	"github.com/spf13/cobra"
	//	"net"
	"ejb1123.tk/FiveMBuilder/magic"
)

var configFile string

// copyCmd represents the copy command
var copyCmd = &cobra.Command{
	Use:   "copy",
	Short: "A brief description of your command",
	Long: `A longer description that spans multiple lines and likely contains examples
and usage of using your command. For example:

Cobra is a CLI library for Go that empowers applications.
This application is a tool to generate the needed files
to quickly create a Cobra application.`,
	Run: func(cmd *cobra.Command, args []string) {
		// TODO: Work your own magic here

		fmt.Println("copy called")
		h:=magic.ReadConfig(configFile)
		fmt.Println(*h)
		files := magic.GetFiles(&h.Server.Src)
		magic.DoCopy(files,&h.Server.Src,&h.Server.Root,&h.Server.ProjectName)
		magic.RestartServer(&h.Server.Url,&h.Server.Password,&h.Server.ProjectName,&h.Server.IceCon)
	},
}

func init() {
	RootCmd.AddCommand(copyCmd)

	copyCmd.Flags().StringVarP(&configFile, "configy", "c", "", "YAML sonfig file")
	/*buildCmd.Flags().StringP("source", "s", "", "the folder to copy files from")
	buildCmd.Flags().StringP("dest","d","","the folder to copy the files to")
	buildCmd.Flags().IntP("port","p",30120,"port for server")
	buildCmd.Flags().String("ip","127.0.0.1","server ip")
	buildCmd.Flags().StringP("password","pass","lovely","server rcon password")*/
}
