var app = getApp();
var baseUrl = app.globalData.baseUrl;

// chart1.html.js
Page({

  /**
   * 页面的初始数据
   */
  data: {
    url: "",
    hiddenLoading: true
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    this.setData({
      url: baseUrl + "pages/chart.html",
      hiddenLoading: false
    })
  },

  /**
   * 生命周期函数--监听页面初次渲染完成
   */
  onReady: function () {
    this.setData({
      hiddenLoading: true
    });
    console.log(123);
  },

  /**
   * 生命周期函数--监听页面显示
   */
  onShow: function () {
    console.log(456);
  },

  /**
   * 生命周期函数--监听页面隐藏
   */
  onHide: function () {

  },

  /**
   * 生命周期函数--监听页面卸载
   */
  onUnload: function () {

  },

  /**
   * 页面相关事件处理函数--监听用户下拉动作
   */
  onPullDownRefresh: function () {

  },

  /**
   * 页面上拉触底事件的处理函数
   */
  onReachBottom: function () {

  },

  /**
   * 用户点击右上角分享
   */
  onShareAppMessage: function () {

  }


})
