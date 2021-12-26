$version = minver
docfx metadata docfx.json
docker build -t savvyio-docfx:$version -f Dockerfile.docfx .
get-childItem -recurse -path api -include *.yml, .manifest | remove-item