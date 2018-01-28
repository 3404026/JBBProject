
var url = 'https://syscotech.cc:12321/api/User/getCategoryCnt';
//var url = 'http://syscotech.cc:12320/api/User/getCategoryCnt';

//var url = 'http://172.21.111.104:12320/api/User/getCategoryCnt';
//var url = 'https://172.21.111.104:12321/api/User/getCategoryCnt';
var myChart;
var paraCode;
var paraName;




function initChartBar(pcode) {
        myChart = echarts.init(document.getElementById('main'));//获取容器
        //异步加载json格式数据JSONP
        if (pcode == 0) {
            paraCode = 0;
            paraName = '产品';
        }
        else {
            paraCode = pcode.pcode;
            paraName = pcode.name;
        }

        $.getJSON(url, { "pcode": paraCode }, function (jsonStrdata) {
            var data = eval('(' + jsonStrdata + ')'); //把后端返回的json数据装入对象obj
            if (data == "")
            {
                return;
            }
            //$("#myloading").empty(); //ajax返回成功，清除loading图标  
            $("#myloading").hide(); //ajax返回成功，清除loading图标  
            var charName = new Array();
            var charValue = new Array();
            //设置完其它的样式，显示一个空的直角坐标轴，然后获取数据后填入数据
            myChart.setOption({
                title: {
                    text: paraName + "成交率",
                    subtext: '从 20170101  至  今天',
                    left: 'center',
                },
                tooltip : {
                        trigger: 'item',
                        formatter: "{a} <br/>{b} : {c} ({d}%)"
                },

                legend: {
                    data: ['成交率']
                },
                xAxis: {
                    data: []
                },
                yAxis: {},
                series: [{
                    name: '成交',
                    type: 'bar',
                    data: []
                }]
            });

            $.each(data,function(i){ 
                charName[i] = data[i].name ;
                charValue[i] = data[i].value ;
            })


            myChart.setOption({
                xAxis: {
                    data: charName
                },
                yAxis: {
                    data: charValue
                },

                legend: {
                    // orient: 'vertical',
                    // top: 'middle',
                    bottom: 100,
                    left: 'right',
                    data: data.data
                },

                series: [{
                    // 根据名字对应到相应的系列
                    name: '关注度',
                    data: data
                }]
            });
        });

        //echarts图表点击跳转  
        myChart.on('click', function (param) {
            //console.log(param);
            initChartBar(param.data);
        });
};


function initChartPie(pcode) {

    myChart = echarts.init(document.getElementById('main'));//获取容器
    //console.log(pcode);
    if (pcode == 0)
        {
        paraCode = 0;
        paraName = '产品';
        }
    else
        {
        paraCode = pcode.pcode;
        paraName = pcode.name;
        }
        


    $.getJSON(url, { "pcode": paraCode }, function (jsonStrdata) {
        var data = eval('(' + jsonStrdata + ')'); //把后端返回的json数据装入对象obj
        if (data == "") {
            return;
        }
        //console.log(data);
        $("#prod_code").val(paraCode);
        $("#prod_code").val(paraName);
     //   $("#prevProd").text("《返回 "+paraName);


        $("#myloading").empty(); //ajax返回成功，清除loading图标  
        var charName = new Array();
        var charValue = new Array();
        //设置完其它的样式，显示一个空的直角坐标轴，然后获取数据后填入数据
        myChart.setOption({
            title: {
                text:  paraName + "关注度",
                subtext: '从 20170101  至  今天',
                left: 'center',
            },

            tooltip: {
                trigger: 'item',
                formatter: "{a} <br/>{b} : {c} ({d}%)"
            },

            series: [{
                name: '关注度',
                type: 'pie',
                data: []
            }]
        });

        $.each(data, function (i) {
            charName[i] = data[i].name;
            charValue[i] = data[i].value;
        })

        myChart.setOption({
            legend: {
                bottom: 20,
                left: 'center',
                data: data
            },
            series: [{
                name: '关注度',
                type: 'pie',
                radius : '65%',
                center: ['50%', '50%'],
                selectedMode: 'single',
                data: data
            }],
            itemStyle: {
                emphasis: {
                    shadowBlur: 10,
                    shadowOffsetX: 0,
                    shadowColor: 'rgba(0, 0, 0, 0.5)'
                }
            }
        });
    });
    //echarts图表点击跳转  
    myChart.on('click', function (param) {
      initChartPie(param.data);
    });
};