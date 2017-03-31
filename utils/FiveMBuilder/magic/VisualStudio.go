package magic

import (
	//"golang.org/x/sys/windows/registry"

	"golang.org/x/sys/windows/registry"
	//"runtime"
	"path"
	"os"
	"path/filepath"
)

func FindBuildTools() map[string]string {
	//fmt.Println(runtime.GOOS, runtime.GOARCH)
	keys, err := registry.OpenKey(registry.LOCAL_MACHINE, `SOFTWARE\WOW6432Node\Microsoft\VisualStudio\SxS\VS7`, registry.READ|registry.QUERY_VALUE)
	if err != nil {
		panic(err)
	}
	k, err := keys.ReadValueNames(-1)
	if err != nil {
		panic(err)
	}
	fin2 := make(map[string]string)

	//println(k.SubKeyCount,k.ValueCount)
	for _, val := range k {
		i := val
		if err != nil {
			panic(err)
		}
		j, _, _ := keys.GetStringValue(val)
		fin2[i] = j

	}
	return fin2
}

func IsVSVersionSupported(m map[string]string) map[string]string {
	newMap := make(map[string]string)
	for key, value := range m {
		newPath := filepath.ToSlash(path.Join(value, "MSBuild\\"+key+`\Bin\MSBuild.exe`))
		if _, err := os.Stat(newPath); os.IsNotExist(err) {
			//fmt.Println("msbuild does nor exist for visual studio " + key)

			///fmt.Println(newPath)
		} else {
			newMap[key] = path.Clean(newPath)
		}
	}
	return newMap
}
