一个 Exceptionless 相关 WebHooks 项目。

# 目的
当Exceptionless触发配置的WebHooks通知类型时，如Error、LogError，发送消息到 DingTalk（钉钉），以便实时知道线上程序运行情况。

# 效果图

![Image text](https://github.com/justmine66/exceptionless-webhooks/blob/master/result.png)

# 步骤
## 1、部署
   选择一种部署方式。
   ### docker

   ``` shell
   docker run -d -p 8000:80 justmine/exceptionless.api.webhook:0.0.0
   ```

### kubernetes（推荐）

[deployment.yml](https://github.com/justmine66/exceptionless-webhooks/blob/master/k8s/web.yml)

``` shell
kubectl apply -f https://github.com/justmine66/exceptionless-webhooks/blob/master/k8s/web.yml;
或
kubectl -n [指定的命名空间] apply -f https://github.com/justmine66/exceptionless-webhooks/blob/master/k8s/web.yml;
```
## 2、配置

请先部署好webhook钩子。

### Exceptionless

Admin => Projects => Integrations => Add Web Hook：

![Image text](https://github.com/justmine66/exceptionless-webhooks/blob/master/config.png)

>注意：如果你的项目Webhook配置选项没有Error、LogEroor类型，说明Exceptionless团队还没有合并我的PR，请先拉取我的项目：[后端](https://github.com/justmine66/Exceptionless)、[前端](https://github.com/justmine66/Exceptionless.UI)，前后端都需要拉取最新项目代码。

# changes

1. 添加容器化部署脚本，支持docker、kubernetes。
2. 扩展事件模型，添加环境、来源等信息。
3. 升级项目为netcoreapp2.1。
4. 优化httpclient使用方式。
5. 添加事件时间本地化设置。

# 如有疑惑，欢迎QQ。
3538307147
