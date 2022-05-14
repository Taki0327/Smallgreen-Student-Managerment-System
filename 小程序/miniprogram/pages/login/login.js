// miniprogram/pages/login/login.js
const app = getApp()
Page({

  /**
   * 页面的初始数据
   */
  data: {
    zt:"由于你是新用户 请先填写绑定信息qwq",
    name:'',
    school:'春田花花幼儿园',
    classid:'1532',
    stuid:'',
    idcard:'',
    wxid:'',
    loading:false,
    loadshow: false,
    loading2: 'loadingshow'
  },
  nameinput: function (e) {
    if ((/^[\u4E00-\u9FA5A-Za-z]+$/.test(e.detail.value))) {
      this.setData({
        name: e.detail.value
      })
    }
    else
    {
      wx.showToast({
        title: '请正确输入姓名！',
        duration: 2000,
        icon: 'none'
      });
    }
  },
  stuidinput: function (e) {
    this.setData({
      stuid: e.detail.value
    })
  },
  idcardinput: function (e) {
    this.setData({
      idcard: e.detail.value
    })
  },
  getinformation:function(e){
    if (!this.data.name == '' && !this.data.school == '' && !this.data.classid == '' && !this.data.stuid == '' && !this.data.idcard=='')
    {
      let that = this;
        wx.showModal({
          title: '二次确认提醒',
          content: '请再次确认是否绑定该微信',
          success(res) {
            that.setData({
                loading:true
            })
            if (res.confirm) {
              //提交注册信息
              wx.cloud.callFunction({
                name: 'reg',
                data: {
                  a: that.data.wxid,
                  b: that.data.name,
                  c: that.data.school,
                  d: that.data.classid,
                  e: that.data.stuid,
                  f: that.data.idcard
                },
                complete: res => {
                  that.setData({
                    loading: false
                  })
                  if (res.result == 0) {
                    wx.showToast({
                      title: '提交失败！',
                      duration: 2000,
                      icon: 'none'
                    });
                  }
                  else if (res.result == 1) {
                    if (that.data.name=='测试')
                    {
                      wx.showToast({
                        title: '欢迎你 测试用户！',
                        duration: 2000,
                        icon: 'success'
                      });
                    }
                    else
                    {
                      wx.showToast({
                        title: '提交成功！',
                        duration: 2000,
                        icon: 'success'
                      });
                    }
                    wx.redirectTo({
                      url: '../check/check',
                    })
                  }
                  else if (Error) {
                    wx.showToast({
                      title: '服务器连接失败！',
                      duration: 2000,
                      icon: 'none'
                    });
                  }
                }
              })
            }
          }
        })
    }
    else
    {
      wx.showToast({
        title: '信息尚未填写完整',
        duration: 2000,
        icon: 'none'
      });
    }
  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    let that=this;
    if (app.globalData.openid==null)
    {
      wx.reLaunch({
        url: '../check/check',
      })
    }
    else
    {
      if (app.globalData.name==22)
      {
        that.setData({
          loadshow: true,
          loading2: 'loadingshow'
        })
        wx.cloud.callFunction({
          name: 'reg',
          data: {
            a: app.globalData.openid,
            b: '0',
            c: '0',
            d: '0',
            e: '0',
            f: '0'
          },
          complete: res => {
            if (res.result!=null)
            {
            var json = JSON.parse(res.result)
            that.setData({
              name: json["Name"],
              school: json["School"],
              classid: json["Classid"],
              stuid: json["Stuid"],
              idcard: json["Idcard"],
              zt:'你的资料审核不通过，请修改后再次提交！',
              loadshow: false,
              loading2: 'loadinghidden'
              })
            }
             else if (Error) {
              wx.showToast({
                title: '服务器连接失败！',
                duration: 2000,
                icon: 'none'
              });
            }
          }
        })
      }
      this.setData({
        wxid: app.globalData.openid
      })
    }
  },
  onShow: function () {
    wx.hideHomeButton();
  },
})