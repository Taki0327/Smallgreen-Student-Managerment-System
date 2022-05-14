// miniprogram/pages/leave/leave.js
const app = getApp()
Page({

  /**
   * 页面的初始数据
   */
  data: {
    msg:'',
    loadshow: false,
    loading: 'loadingshow',
    loading2:false
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    if (app.globalData.openid == null) {
      wx.reLaunch({
        url: '../check/check',
      })
    }
    else
    {
      if (app.globalData.sign == 3)
      {
        app.globalData.sign = 0
        this.setData({
          loadshow: true,
          loading: 'loadingshow'
        })
        let that = this;
        wx.cloud.callFunction({
          name: 'sign',
          data: {
            a: app.globalData.openid,
            b: "0",
            c: 'leaveleave'
          },
          complete: res => {
            
            if (res.result !=null) {
              that.setData({
               msg:res.result,
               loadshow: false,
               loading: 'loadinghidden'
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
  },
  reasontext: function (e) {
    app.globalData.reason = e.detail.value;
  },
  leave:function(e)
  {
    this.setData({
      loading2:true
    })
    if (app.globalData.reason != null && app.globalData.reason != " ")
    {
      let that=this;
      wx.cloud.callFunction({
        name: 'sign',
        data: {
          a: app.globalData.openid,
          b: app.globalData.reason,
          c: 'leave'
        },
        complete: res => {
            if(res.result=="sucess")
            {
              that.setData({
                loading2: false
              })
              wx.showToast({
                title: '提交成功！',
                duration: 2000,
                icon: 'success'
              });
              wx.reLaunch({
                url: '../sign/sign'
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
    else
    {
      wx.showToast({
        title: '你还没有输入文字喔！',
        duration: 2000,
        icon: 'none'
      });
    }
  },
  /**
   * 生命周期函数--监听页面显示
   */
  onShow: function () {
    wx.hideHomeButton();
  }
})