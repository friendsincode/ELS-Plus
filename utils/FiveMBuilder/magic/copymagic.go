package magic

import (
	"fmt"
	"io/ioutil"
	"log"
	"os"
	"gopkg.in/yaml.v2"
	"net"
	"path/filepath"
	"path"
	"io"
)

func GetFiles(src *string) *Tempfiles {

	_, err := os.Stat(*src)
	if err != nil {
		fmt.Println("err")
	}

	files := Tempfiles{}
	filepath.Walk(*src, func(path string, info os.FileInfo, err error) error {
		if v, _ := os.Stat(path); v.IsDir() {
			tpath, _ := filepath.Rel(*src, path)
			if tpath != "." {
				files.folders = append(files.folders, tpath)
			}
		}
		if v, _ := os.Stat(path); !v.Mode().IsDir() && !v.IsDir() {
			tpath, _ := filepath.Rel(*src, path)
			files.files = append(files.files, tpath)
		}
		//files = append(files, path)
		return nil
	})
	fmt.Println(files)
	return &files
}

func DoCopy(tempfiles *Tempfiles, src *string, root *string, projectName *string) {
	newpath := path.Join(*root, "resources", *projectName)

	if _, err := os.Stat(newpath); os.IsNotExist(err) {
		os.Mkdir(newpath, os.ModeDir)
	}
	for _, q := range tempfiles.files {
		pathn := filepath.FromSlash(path.Join(filepath.FromSlash(newpath), filepath.FromSlash(q)))
		fmt.Println(pathn)
		if v, err := os.Stat(pathn); os.IsNotExist(err) || v.Mode().IsRegular() {
			os.Remove(pathn)
		}
		if filepath.Dir(q) != "." {
			os.MkdirAll(filepath.Dir(pathn), os.ModeDir)
		}
		if _, err := os.Stat(pathn); os.IsNotExist(err) {
			srcfl, _ := os.Open(path.Join(*src, q))
			dst, _ := os.Create(pathn)
			_, err := io.Copy(dst, srcfl)
			if err != nil {
				panic(err)
			}
			fmt.Println("written:" + pathn)
		}
	}
}

type Tempfiles struct {
	folders []string
	files   []string
}

type T struct {
	Server struct {
		Enabled     bool
		Url         net.IP `yaml:"url"`
		Password    string
		Src         string
		Root        string
		ProjectName string `yaml:"name"`
	}`yaml:"server"`
}

func ReadConfig(config string) *T {
	result, err := ioutil.ReadFile(config)
	if err != nil {
		log.Fatal(err)
		panic(err)
	}
	s := string(result)
	fmt.Println(s)
	g := T{}
	yaml.Unmarshal(result, &g)
	if g.Server.Enabled == false {
		os.Exit(0)
	}
	return &g
}
