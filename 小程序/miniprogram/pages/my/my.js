// miniprogram/pages/my/my.js
const app = getApp()
Page({

  /**
   * 页面的初始数据
   */
  data: {
    avatarUrl: '',
    name: '',
    userid:'',
    school:'',
    classid:'',
    stuid:'',
    sex:'',
    birth:'',
    idcard:'',
    signnum:'',
    latenum:'',
    loadshow: true,
    loading: 'loadingshow'
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
    else{
      this.setData({
        avatarUrl: app.globalData.avatarUrl,
        name: app.globalData.name,
        loadshow: true,
        loading: 'loadingshow'
      })
      var that = this;
      wx.cloud.callFunction({
        name: 'about',
        data: {
          a: app.globalData.openid,
          b: "my",
        },
        complete: res => {
          var json = JSON.parse(res.result)
          that.setData({
            userid: json["Userid"],
            school: json["School"],
            classid: json["Classid"],
            stuid: json["Stuid"],
            sex: json["Sex"],
            birth: json["Birth"],
            idcard: json["Idcard"],
            signnum: json["Signnum"],
            latenum: json["Latenum"],
            loadshow: false,
            loading: 'loadinghidden'
          })
        }
      })
    }
  },
about:function()
{
  wx.navigateTo({
    url: '../about/about'
  })
},
  /**
   * 生命周期函数--监听页面显示
   */
  onShow: function () {
    wx.hideHomeButton();
  }
})