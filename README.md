# YAAM

Basic console application to get a grasp of the Linux [proc](https://man7.org/linux/man-pages/man5/proc.5.html) implmentation.

## Run debug

```console
dotnet run --configuration Debug
```

## Publish

### Linux

```console
dotnet publish \
    --runtime "linux-x64" \
    --self-contained true \
    --configuration "Release" \
    --output "./Publish/linux/" \
    -p:PublishSingleFile=true
```

### Windows

```console
dotnet publish \
    --runtime "win-x64" \
    --self-contained true \
    --configuration "Release" \
    --output "./Publish/win/" \
    -p:PublishSingleFile=true
```

### MacOS

```console
dotnet publish \
    --runtime "osx-x64" \
    --self-contained true \
    --configuration "Release" \
    --output "./Publish/osx/" \
    -p:PublishSingleFile=true
```