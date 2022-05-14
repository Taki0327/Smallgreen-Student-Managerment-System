// 云函数入口文件
const cloud = require('wx-server-sdk')
var request = require('request')
cloud.init()
// 云函入口函数
exports.main = async (event, context) => new Promise((resolve, reject) => {
  request.post({
    url: 'http://39.99.195.210/abc/WX/Reg',
    formData: {
      "wxid": event.a,
      "name": event.b,
      "school": event.c,
      "classid": event.d,
      "stuid": event.e,
      "idcard": event.f,
      "type": "reg"
    }
  }, function (error, response, body) {
    resolve(body)
  })
})