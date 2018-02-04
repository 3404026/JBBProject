
var baseUrl = 'https://syscotech.cc:12321/api/User/';
//var baseUrl = 'http://syscotech.cc:12320/api/User/';
//var baseUrl = 'http://172.21.111.104:12320/api/User/';
//var baseUrl = 'https://172.21.111.104:12321/api/User/';
//var baseUrl = 'http://192.168.0.189:12320/api/User/';
//var baseUrl = 'https://172.21.111.104:12321/api/User/';


var myChart;
var paraCode;
var paraName;

function initInventoryChartBar() {
    var intInventoryCnt = 0;
    myChart = echarts.init(document.getElementById('main'));//获取容器
    $.getJSON(baseUrl + "getInventoryByCaizhi", function (jsonStrdata) {
        var data = eval('(' + jsonStrdata + ')'); //把后端返回的json数据装入对象obj
        if (data == "") {
            return;
        }
        //$("#myloading").empty(); //ajax返回成功，清除loading图标  
        $("#myloading").hide(); //ajax返回成功，清除loading图标  
        var charName = new Array();
        var charValue = new Array();
        //设置完其它的样式，显示一个空的直角坐标轴，然后获取数据后填入数据
        myChart.setOption({
            title: {
                //text: paraName + "成交率",
                //subtext: '从 20170101  至  今天',
                left: 'center',
            },
            tooltip: {
                trigger: 'item',
                formatter: "{a} <br/>{b} : {c}"
            },

            xAxis: {
                data: []
            },
            yAxis: {},
            series: [{
                name: '材质 : 件数',
                type: 'bar',
                data: []
            }]
        });

        $.each(data, function (i) {
            charName[i] = data[i].name;
            charValue[i] = data[i].value;
            intInventoryCnt = intInventoryCnt + data[i].value;
        })
        //console.log(intInventoryCnt);
        document.getElementById('title_bar').innerHTML = "<span style='background-color:#6699ff'>当前在柜商品数量<font color=red>" + intInventoryCnt + "</font>件</span>";


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
                name: '材质 : 件数',
                data: data
            }]
        });
    });

    //echarts图表点击跳转  
    myChart.on('click', function (param) {
        //console.log(param);
        window.location.href = "inventoryItem.html?pname=" + param.data.name + "&pcode=" + param.data.pcode;
        //initChartBar(param.data);
    });
};


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

        $.getJSON(baseUrl + "getCategoryCnt", { "pcode": paraCode }, function (jsonStrdata) {
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
                    left: 'center',
                },
                tooltip : {
                        trigger: 'item',
                        formatter: "{a} <br/>{b} : {c}"
                },

                legend: {
                    data: ['成交率']
                },
                xAxis: {
                    data: []
                },
                yAxis: {},
                series: [{
                    name: '成交率',
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
                    name: '成交率',
                    data: data
                }]
            });
        });

        //echarts图表点击跳转  
        //myChart.on('click', function (param) {
        //    //console.log(param);
        //    initChartBar(param.data);
        //});
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
        


    $.getJSON(baseUrl + "getCategoryCnt", { "pcode": paraCode }, function (jsonStrdata) {
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
                subtext: '截至今日',
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


UrlParm = function () { // url参数    
    var data, index;
    (function init() {
        data = [];
        index = {};
        var u = window.location.search.substr(1);
        if (u != '') {
            var parms = decodeURIComponent(u).split('&');
            for (var i = 0, len = parms.length; i < len; i++) {
                if (parms[i] != '') {
                    var p = parms[i].split("=");
                    if (p.length == 1 || (p.length == 2 && p[1] == '')) {// p | p=    
                        data.push(['']);
                        index[p[0]] = data.length - 1;
                    } else if (typeof (p[0]) == 'undefined' || p[0] == '') { // =c | =    
                        data[0] = [p[1]];
                    } else if (typeof (index[p[0]]) == 'undefined') { // c=aaa    
                        data.push([p[1]]);
                        index[p[0]] = data.length - 1;
                    } else {// c=aaa    
                        data[index[p[0]]].push(p[1]);
                    }
                }
            }
        }
    })();
    return {
        // 获得参数,类似request.getParameter()    
        parm: function (o) { // o: 参数名或者参数次序    
            try {
                return (typeof (o) == 'number' ? data[o][0] : data[index[o]][0]);
            } catch (e) {
            }
        },
        //获得参数组, 类似request.getParameterValues()    
        parmValues: function (o) { //  o: 参数名或者参数次序    
            try {
                return (typeof (o) == 'number' ? data[o] : data[index[o]]);
            } catch (e) { }
        },
        //是否含有parmName参数    
        hasParm: function (parmName) {
            return typeof (parmName) == 'string' ? typeof (index[parmName]) != 'undefined' : false;
        },
        // 获得参数Map ,类似request.getParameterMap()    
        parmMap: function () {
            var map = {};
            try {
                for (var p in index) { map[p] = data[index[p]]; }
            } catch (e) { }
            return map;
        }
    }
}();



function initInventoryByPeidaiChartBar(pcode) {
    var intInventoryCnt = 0;
    myChart = echarts.init(document.getElementById('main'));//获取容器
    $.getJSON(baseUrl + "getInventoryByPeidai", { "pcode": pcode }, function (jsonStrdata) {
        var data = eval('(' + jsonStrdata + ')'); //把后端返回的json数据装入对象obj
        if (data == "") {
            window.location.href = "inventory.html";
            return;
        }
        //$("#myloading").empty(); //ajax返回成功，清除loading图标  
        $("#myloading").hide(); //ajax返回成功，清除loading图标  
        var charPeidaiName = new Array();
        var charjianShu = new Array();
        var charciShu = new Array();
        var chartimeCount = new Array();
        var objSeries = new Array();
        var strjianShu;
        var strciShu;
        var strTimeCount;

        $.each(data, function (i) {
            charPeidaiName[i] = data[i].peidaiName;
            charjianShu[i] = data[i].jianShu;
            charciShu[i] = data[i].ciShu;
            //chartimeCount[i] = Math.round(data[i].timeCount / 60 * 100) / 100;
            chartimeCount[i] = data[i].timeCount;
            console.log(data[i].timeCount);

        })
        

        //设置完其它的样式，显示一个空的直角坐标轴，然后获取数据后填入数据
        myChart.setOption({
            tooltip: {
                trigger: 'axis',
                axisPointer: {            // 坐标轴指示器，坐标轴触发有效
                    type: 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
                }
            },
            legend: {
                data: ['件数', '体验次数', '体验时长(分钟)']
            },
            grid: {
                left: '3%',
                right: '4%',
                bottom: '3%',
                containLabel: true
            },
            xAxis: {
                type: 'value'
            },
            yAxis: {
                type: 'category',
                data: charPeidaiName
            },
            series: [
                { name: "件数", type: "bar", stack: "总量", label: { normal: { show: true, position: "insideRight" } }, data: charjianShu },
                { name: "体验次数", type: "bar", stack: "总量", label: { normal: { show: true, position: "insideRight" } }, data: charciShu },
                { name: "体验时长(分钟)", type: "bar", stack: "总量", label: { normal: { show: true, position: "insideRight" } }, data: chartimeCount }
            ]

        });
    });

};



