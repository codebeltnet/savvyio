$version = minver -i
docfx metadata docfx.json
docker buildx build -t savvyio-docfx:$version --platform linux/arm64,linux/amd64 --load -f Dockerfile.docfx .
get-childItem -recurse -path api -include *.yml, .manifest | remove-item
