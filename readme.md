# interface design

## 用户资源

### /api/login

登陆接口

提交方法 : POST

提交参数 : 

```json
POST /api/login

{
    "userid": "___", // 这里存放你的手机号
    "password": "___",
}
```

返回内容：

登陆错误:

* 用户不存在：404
* 账号密码错误：401

登陆成功：

```json
{
    "token": "ldjaskhdaskfgh",
    "timeout": "___",	// 过期，单位分钟
    "userID": ""		// 用户的 UID, 数字
}
```

注意：返回的token类型是bearer token, 以后放在 HTTP Header auth 字段中

### /api/Users

#### 注册/添加用户

这个你们自己补充0.0？

```json
POST /api/users
POST /api/register 	// 俩者都可以

{
    "phoneNumber": "...",
    "NickName": "...",
    "Password": "...",
}
```

注册失败:

* 重复手机号/用户名：409，返回 消息用户名／电话号码重复

```json
409
"用户名重复"
```

成功返回 HTTP 204:

```json
HTTP 204
// 在 Response Header Location 字段返回新添加User的资源
Location: /api/users/{new_user_id}
```

#### 获得用户信息

用户的schema:

```json
{
    "UserID": 1,
    "NickName": "mwish",
    "RealName": "空酱",	
    "PhoneNumber": "18817281365",
    "GenderString": "女", // 性别字符串
    "BirthDay": "0001-01-01T00:00:00",
    "BirthDayDate": "01/01/0001",
    "Local": null,
    "Home": null
}
```



##### GET /api/Users?name=\_\_

获得有名称的User, 缺少name返回400, user 不存在返回404

##### GET /api/Users/{UserID:int}

同上,  不存在返回404

##### POST /api/Users ：-> 正文中放入以上Schema

* 如果 User 存在：303

  ```json
  303
  Location: /api/Users/{UserID}	//新资源
  ```

* 如果 User 不存在: 302 重定向到： /api/register



## 商品和目录相关

### /api/Categories

#### Schema

```json
{
    "CategoryID" : "", // int uid
    "Name": ""	// name of category
}
```



#### GET /api/Categories?kw=\_\_&name=\_\_&pn=\_\_

```json
200

{
    "result_num": 2,
    "categories": [
        {},
        {},
        ...
    ],
    "page_num": 1
}
```

kw 表示key word, 用于搜索对应的对象

name 是具体的名字

没有则按分页返回

pn 是页面数目，默认为1

#### GET /api/catagory/{id}

获得对应的种类信息(这玩意真的可靠么) 当然这玩意很不可靠

#### PUT: api/Categories/5 

单个更新: 需要登录

*暂无*：权限

### /api/GoodEntities

#### Schema

```json
{
    "GoodEntityID": 23, // int id
    "GoodName": "dasl", // 商品名称
    "Brief": "",		// 简介
    "Detail": "",		// 详情
    "Stock": 9,		// 库存
    "SellProvince": "",	// 寄出的省份
    "GoodEntityState": 1, // 状态：1 销售 2 下架 3 失效
    // 关联的 Attributes 表
    "Attributes": [
        {
            "GoodAttributeID": 3
            "GoodAttributeName": "等级",
            "GoodAttributeOptions": [
                {
                    "Describe":"一星",
                    "GoodAttributeID":  3
                },
                {
                    "Describe":"俩星",
                    "GoodAttributeID": 3
                },
        		{
                    "Describe":"三星",
                    "GoodAttributeID": 3
                },
            ]
        },
    ]
}
```

SaleEntities

```json
{
    "ID": 213,
    "Amount": 2213, // 总量
    // 实际上不存在这段，但是我弄成这样了
    "AttributeOptionData": [
        {
            "Describe":"一星",
            "GoodAttributeID": 3
            "GoodAttributeName": "等级"
        },
      	{},
        {}
    ]
}
```





### GoodEntity 查询

方法统一为GET 

#### GET /api/GoodEntities?Pn=\_\_&Kw=\_\_

查询，pn 默认为1， kw 不可或缺

```
{
    "result_num": 2,
    "entities": [
        {},
        {},
        ...
    ],
    "page_num": 1
}
```



#### GET /api/Catagories/{id}/GoodEntities?pn=\_\_

获得对应种类的商品信息，pn默认为1，返回：

```
200

{
    "result_num": 2,
    "entities": [
        {},
        {},
        ...
    ],
    "page_num": 1
}
```

注意分页

### GET /api/goods?kw=\_\_&pn=\_\_

查询所有货物

----

查询返回信息

HTTP 422 查询字段错误

HTTP 404 我查你🐎呢

HTTP 400 参数有问题

描述信息...我暂时不知道写啥

```json
HTTP 200

{
    "result_num": 20,
    "items" : [
        {},
        {},
    ],
    "page_num": 3
}
```

## 商品评论

### Comment: 单个商品所有评论

### CommentInfo 单个评论

```json
CommentInfo
{
    "CommentID": 5
    "Detail": "DALHDA",
    "LevelRank": 1, // 0 没有评价，1差评 2中评 3好评
    "UserCommentTime":"dd/MM/yyyy h-m-s" // 评论时间
}
```



#### GET /api/GoodEntities/{GoodID}/Comments?pn=\_\_

pn  默认为1，显示评论

```
{
    "result_num": 28, 
    "CommentInfos": [
        {}
    ]
}
```



## 购物车管理

所有对购物车的访问需要：

```
Headers:
	Authentication: Bearer + jwt
```



#### Schema

```json
{
    "UserID": 13, // userID
    "SalesEntities": [
        {},
        {},
        {}
    ]
}
```



添加商品

#### POST /api/Carts/SalesEntities/{good\_id}

把id为`good_id`的商品加入自己的`cart`. 

POST 200 表示加入成功，403表示不存在

DELETE 表示删除 204 成功

####  GET /api/Carts

获得 Schema那样的数据

## 订单管理

### Schema

```json
{
    "OrderformID": 233,
    "TransacDateTime": "dd/MM/yyyy h-m-s", // 交易时间
    "State": 1,	// 状态 : 已完成0／已发货1／待支付2
    "UserID": 213,
    "TotalPrice": 21.4,
    "SaleEntities": [
        // sale entity 的结构
    ]
}
```



## 参考

https://stackoverflow.com/questions/207477/restful-url-design-for-search

https://devcenter.kinvey.com/rest/guides/users



