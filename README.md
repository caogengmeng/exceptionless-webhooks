# exceptionless-webhooks
一个 Exceptionless 相关 WebHooks 项目。

# 流程
当Exceptionless触发配置的WebHooks通知类型时，如Error、LogError，发送消息到 DingTalk（钉钉），以便实时知晓线上程序运行情况。

# 部署
## docker
docker run -d -p 8000:80 justmine/exceptionless.api.webhook:0.0.0

## kubernetes（推荐）
```shell
apiVersion: apps/v1
kind: Deployment
metadata:
  namespace: geekbuying-light-addons
  name: exceptionless-api-weebhook
  labels:
    app: exceptionless-api-weebhook
spec:
  replicas: 1
  selector:
    matchLabels:
      app: exceptionless-api-weebhook
  template:
    metadata:
      labels:
        app: exceptionless-api-weebhook
    spec:
      containers:
        - name: exceptionless-api-weebhook
          image: "justmine/exceptionless.api.webhook:0.0.0"
          imagePullPolicy: IfNotPresent
          ports:
            - name: http
              containerPort: 80
              protocol: TCP

---
apiVersion: v1
kind: Service
metadata:
  namespace: geekbuying-light-addons
  name: exceptionless-api-weebhook
  labels:
    app: exceptionless-api-weebhook
spec:
  type: ClusterIP
  ports:
    - port: 80
      targetPort: http
      protocol: TCP
      name: http
  selector:
    app: exceptionless-api-weebhook
```

# changes

1. 添加容器化部署脚本，支持docker、kubernetes。
2. 扩展事件模型，添加环境、来源等信息。
3. 升级项目为netcoreapp2.1。
4. 优化httpclient使用方式。
5. 添加事件时间本地化设置。
