// miniprogram/pages/check/check.js
const app = getApp()
Page({

  /**
   * 页面的初始数据
   */
  data: {
    avatarUrl: '',
    userInfo: {},
    logged: false,
    name: '正在获取昵称中···',
    zt: "正在检测账号权限中~",
    show:false,
    type:''
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function () {
    this.check()
  },
  onGetUserInfo: function (e) {
    if (!this.data.logged && e.detail.userInfo) {
      this.check(),
      this.setData({
        logged: true,
        avatarUrl: e.detail.userInfo.avatarUrl,
        name: e.detail.userInfo.nickName,
        zt: "正在加载中qwq",
        show: false
      })
    }
  },
  check:function()
  {
    var that = this;
    // 获取用户信息
    wx.getSetting({
      success: res => {
        if (res.authSetting['scope.userInfo']) {
          // 已经授权，可以直接调用 getUserInfo 获取头像昵称，不会弹框
          wx.getUserInfo({
            success: res => {
              app.globalData.avatarUrl = res.userInfo.avatarUrl,
                app.globalData.name = res.userInfo.nickName,
                this.setData({
                  avatarUrl: app.globalData.avatarUrl,
                  name: app.globalData.name
                }),
                wx.cloud.callFunction({
                  name: 'wxid',
                  success: res => {
                    app.globalData.openid = res.result.openid,
                      //取微信id云函数
                      wx.cloud.callFunction({
                        name: 'top',
                        data: {
                          a: app.globalData.openid
                        },
                        complete: res => {
                          if (res.result == 0) {
                            this.setData({
                              zt: "你还没有注册 正在跳转至注册页面QWQ"
                            }),
                              wx.redirectTo({
                                url: '../login/login',
                              })
                          }
                          else if (res.result != null) {
                            var json = JSON.parse(res.result)
                            app.globalData.name = json["Name"]
                            if (json["Type"] == 0)
                            {
                              app.globalData.name=22;
                              wx.redirectTo({
                                url: '../login/login'
                              })
                            }
                            else if (json["Type"] == 1) {
                              this.setData({
                                zt: "你已经提交了注册资料 正在审核中QWQ~",
                                name: app.globalData.name
                              })
                            }
                            else if (json["Type"] == 2) {
                              app.globalData.sign = 0
                              this.setData({
                                zt: "欢迎进入学生管理系统~",
                                name: app.globalData.name
                              }),
                                wx.showToast({
                                  title: '你今天还没有签到喔qwq',
                                  icon: 'none',
                                  duration: 2000
                                }),
                                wx.switchTab({
                                  url: '../sign/sign',
                                })
                            }
                            else if (json["Type"] == 3) {
                              app.globalData.sign = 1
                              this.setData({
                                zt: "欢迎进入学生管理系统~",
                                name: app.globalData.name
                              }),
                                wx.switchTab({
                                  url: '../index/index'
                                })
                            }
                          }
                          else if (Error) {
                            this.setData({
                              zt: "服务器连接失败 请稍后重试QAQ"
                            })
                          }
                        }
                      })
                  }
                })
            }
          })
        }
        else {
          wx.getUserInfo({
            success: res => {
            }
          })
          this.setData({
            name: '欢迎访问学生管理系统',
            zt: "请点击下方我要登录按钮以使用本程序",
            show: true
          })
        }
      },
    })
  }
})