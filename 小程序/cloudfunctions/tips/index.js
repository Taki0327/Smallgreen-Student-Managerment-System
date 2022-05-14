const cloud = require('wx-server-sdk')
cloud.init()
exports.main = async (event, context) => {
  try {
    const result = await cloud.openapi.subscribeMessage.send({
      touser: event.wxid,
      page: 'pages/sign/sign',
      data: {
        phrase1: {
          value: event.state,
        },
        name2: {
          value: event.name,
        },
        date3: {
          value: event.signtime,
        }
      },
      templateId: 'qPeZSdJMCxab8xV0zozg7kBZ3cSFcdE_v-cHPy14Es0'
    })
    console.log(result)
    return result
  } catch (err) {
    console.log(err)
    return err
  }
}