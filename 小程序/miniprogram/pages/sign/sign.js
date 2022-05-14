const app = getApp()
Page({
  data: {
    latitude: "",
    longitude: "",
    markers: [],
    circles: [],
    school:'',
    classid:'',
    date:'',
    week:'',
    sign:'',
    showsign: false,
    showleave: false,
    signtime:'',
    setadd:'',
    settime:'',
    imgsrc:'../images/nosign.png',
    loadshow:true,
    loading:'loadingshow'
  },

  onLoad: function () {
    
    if (app.globalData.openid == null) {
      wx.reLaunch({
        url: '../check/check',
      })
    }
    else
    {
      this.getinformation();
      this.fresh();
    }
  },
  getinformation: function (e)
  {
    this.setData({
      loadshow: true,
      loading: 'loadingshow'
    })
    var that = this;
    wx.cloud.callFunction({
      name: 'sign',
      data: {
        a: app.globalData.openid,
        b: app.globalData.sign,
        c: 'sign'
      },
      complete: res => {
        var json = JSON.parse(res.result)
        if (app.globalData.sign == 1) {
          that.setData({
            sign: "已签到",
            signtime: json["Signtime"],
            school: json["School"],
            classid: json["Classid"],
            date: json["Date"],
            week: json["Week"],
            showsign: false,
            showleave: false,
            imgsrc: '../images/issign.png'
          })
        }
        else if (json["Type"] == "1") {
          that.setData({
            sign: "你的请假审批已经通过，无需签到。",
            signtime: '',
            showleave: false,
            showsign: false,
            imgsrc: '../images/issign.png'
          })
        }
        else if (app.globalData.sign == 0) {
          that.setData({
            sign: "等待签到",
            school: json["School"],
            classid: json["Classid"],
            date: json["Date"],
            week: json["Week"],
            settime: json["Settime"],
            setadd: json["Setadd"],
            signtime: json["Settime"] + ' 点前',
            imgsrc: '../images/nosign.png'
          })
          if (json["Type"] == "0") {
            that.setData({
              sign: "你已提交请假申请 教师正在审核中···",
              signtime:'',
              showleave: false,
              showsign: false
            })
          }
          else if (json["Type"] == "2") {
            app.globalData.sign = 3;
            that.setData({
              sign: "你的请假审批未通过，请签到或修改后重新提交!!!",
              showsign: true,
              showleave: true
            })
            wx.navigateTo({
              url: '../leave/leave'
            })
          }
          else
          {
            that.setData({
              showsign: true,
              showleave: true
            })
          }
        }
        else{
          wx.showToast({
            title: '服务器连接失败！',
            duration: 2000,
            icon: 'none'
          });
        }
        that.setData({
          loadshow: false,
          loading: 'loadinghidden'
        })
      }
    })
  },
  fresh:function(e){
    let that=this;
    wx.getSetting({
      success(res) {
        if (res.authSetting['scope.userLocation']) {
          wx.getLocation({
            type: 'gcj02', // 默认为 wgs84 返回 gps 坐标，gcj02 返回可用于 wx.openLocation 的坐标
            success: function (res) {
              that.setData({
                latitude: res.latitude,
                longitude: res.longitude,
                markers: [{
                  id: "签到点",
                  latitude: res.latitude,
                  longitude: res.longitude,
                }],
                circles: [{
                  latitude: res.latitude,
                  longitude: res.longitude,
                  fillColor: '#7cb5ec88',
                  radius: 200,
                  strokeWidth: 0
                }]
              })
            },
            fail: function (res) {
              wx.showModal({
                title: '自动获取位置信息失败',
                content: '请打开手机定位(GPS/位置信息)开关 然后点击重新获取按钮'
              })
            }
          })
        }
        else {
          wx.getLocation({
            type: 'gcj02', // 默认为 wgs84 返回 gps 坐标，gcj02 返回可用于 wx.openLocation 的坐标
            success: function (res) {
              that.setData({
                latitude: res.latitude,
                longitude: res.longitude,
                markers: [{
                  id: "签到点",
                  latitude: res.latitude,
                  longitude: res.longitude,
                }],
                circles: [{
                  latitude: res.latitude,
                  longitude: res.longitude,
                  fillColor: '#7cb5ec88',
                  radius: 200,
                  strokeWidth: 0
                }]
              })
            },
            fail: function (res) {
              wx.showModal({
                title: '请允许小程序获取你的位置信息',
                content: '位置信息仅在本地判断 不会保存或上传 请放心授权qwq',
                success: function (res) {
                  if (res.confirm) {
                    wx.openSetting({
                      success: function (data) {
                        if (data.authSetting["scope.userLocation"] == true) {
                          wx.showToast({
                            title: '授权成功',
                            icon: 'success',
                            duration: 2000
                          })
                          this.fresh();
                        }
                      }
                    })
                  }
                  else {
                    wx.showToast({
                      title: '授权失败',
                      duration: 2000
                    })
                  }
                }
              })
            }
          })
         
        }
      }
    })
  },
  onShow: function () {
    wx.hideHomeButton();
  },
  sign:function()
  {
    let that = this; 
    wx.requestSubscribeMessage({
      tmplIds: ["qPeZSdJMCxab8xV0zozg7kBZ3cSFcdE_v-cHPy14Es0"],
      success: function (res) {
        that.signsign();
        console.log(res)
      },
      fail: function (res) {
        that.signsign();
        console.log(res)
      },
    })
    
  },
  signsign: function ()
  {
    let that = this;
    var type = "";
    if (this.setadd == 0) {
      type = "ok";
    }
    wx.cloud.callFunction({
      name: 'sign',
      data: {
        a: app.globalData.openid,
        b: app.globalData.sign,
        c: "ok"
      },
      complete: res => {

        if (res.result == "sucess") {
          wx.showToast({
            title: '签到成功',
            icon: 'success',
            duration: 2000
          })
          app.globalData.sign = 1;
          that.setData({
            showsign: false,
            showleave: false,
            sign: "已签到",
            signtime: "",
            imgsrc: '../images/issign.png'
          })
          that.getinformation('签到成功');
        }
        else if (res.result == "fail") {
          wx.showToast({
            title: '你今天迟到了喔！',
            icon: 'none',
            duration: 2000
          })
          app.globalData.sign = 1;
          that.setData({
            showsign: false,
            showleave: false,
            sign: "已签到",
            signtime: "",
            imgsrc: '../images/issign.png'
          })
          that.getinformation('迟到');
        }
        else if (Error || res.result == "") {
          wx.showToast({
            title: '服务器连接失败 请稍后重试QAQ',
            icon: 'none',
            duration: 2000
          })
        }
      }
    })
  },
  leave:function(e)
  {
    wx.navigateTo({
      url: '../leave/leave'
    })
  }
})