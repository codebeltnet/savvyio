$version = minver -i -t v -v w
docker tag savvyio-docfx:$version jcr.codebelt.net/geekle/savvyio-docfx:$version
docker push jcr.codebelt.net/geekle/savvyio-docfx:$version
