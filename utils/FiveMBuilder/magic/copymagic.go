package magic

import (
	"fmt"
	"io/ioutil"
	"log"
	"os"
	"gopkg.in/yaml.v2"
	"path/filepath"
	"path"
	"io"
	"os/exec"
	"time"
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
		if v, _ := os.Stat(path); v.Mode().IsRegular() {
			tpath, _ := filepath.Rel(*src, path)
			files.files = append(files.files, tpath)
		}
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
			dst.Close()
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
		Url         string `yaml:"url"`
		Password    string
		Src         string
		Root        string
		ProjectName string `yaml:"name"`
		IceCon      string        `yaml:"iceconpath"`
	}`yaml:"server"`
}

func ReadConfig(config string) *T {
	result, err := ioutil.ReadFile(config)
	if err != nil {
		log.Fatal(err)
		panic(err)
	}

	g := T{}
	yaml.Unmarshal(result, &g)
	if g.Server.Enabled == false {
		os.Exit(0)
	}
	if !filepath.IsAbs(g.Server.Src) {
		pathh, err := filepath.Abs(g.Server.Src)
		if err != nil {
			panic(err)
		}
		g.Server.Src = pathh
	}
	if !filepath.IsAbs(g.Server.IceCon) {
		pathh, err := filepath.Abs(g.Server.IceCon)
		if err != nil {
			panic(err)
		}
		g.Server.IceCon = pathh
	}
	return &g
}

func RestartServer(url *string, password *string, projectName *string, iceconPath *string) {
	time.Sleep(1000)
	cmdd := exec.Command(*iceconPath, "-c restart " + *projectName, *url, *password)
	cmdd.Stdout = os.Stdout
	//hhh,_:=cmdd.Output()
	//fmt.Println(hhh)
	cmdd.Stderr = os.Stderr
	cmdd.Run()
}
