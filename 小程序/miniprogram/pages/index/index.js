// miniprogram/pages/index/index.js
const app = getApp()
Page({

  /**
   * 页面的初始数据
   */
  data: {
    wxid:'',
    msg:'fawefgef',
    time:'2019.12.16 19.34',
    people:'XX老师',
    content: [],
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
    else {
      this.setData({
        loadshow: true,
        loading: 'loadingshow'
      })
      this.fresh();
    }
  },
  fresh:function(){
    let that=this;
    wx.cloud.callFunction({
      name: 'index',
      data: {
      },
      complete: res => {
        if(res.result!=null)
        {
          that.setData({
              content: JSON.parse(JSON.parse(res.result)),
              loadshow: false,
              loading: 'loadinghidden'
          })
        }
      }
    })
},
  /**
   * 生命周期函数--监听页面显示
   */
  onShow: function () {
    wx.hideHomeButton();
  },

  /**
   * 页面相关事件处理函数--监听用户下拉动作
   */
  onPullDownRefresh: function () {
    this.fresh();
    setTimeout(function () {
      wx.hideNavigationBarLoading() //完成停止加载
      wx.stopPullDownRefresh() //停止下拉刷新
    }, 1000);
  }
})