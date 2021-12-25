
/**
 * @authors 全局对象 | 依赖于 jquery.js 及 tools.js工具包 及 jquery.common.js插件包
 * @date    2015-08-26 15:55:16
 * @version 谭子良
 */

/* 
 * *************************************************************
 * 
 * 下面公共全局对象
 * 
 * *************************************************************
 */

;(function(){

	/*
     * 全局对象定义
     */
	window.P = {};

	/*
     * 通用加载简单的编辑器
     */
	P.LoadSimpleEditor = function(htmlID,areaHeight,uploadUrl,fileManagerJson){
	    var KindEditor = window.KindEditor;
	    var defaultVal = $(htmlID).val();
	    KindEditor.ready(function(K) {
	        window.editor = K.create(htmlID, {
	  	        width: '99%',
	  	        height: areaHeight,
	  	        items:['clearhtml','source', 'preview','undo','redo','|','formatblock','fontname', 'fontsize', '|', 'forecolor', 'hilitecolor',  '|', 'bold','italic', 'underline',  '|', 'lineheight', 'removeformat', '|','selectall','justifyleft', 'justifycenter', 'justifyright','justifyfull','|','image', 'multiimage','subscript','superscript','|', 'insertorderedlist', 'insertunorderedlist','indent','outdent','|','fullscreen','|', 'about'],
	  	        resizeType:0,
	            uploadJson :uploadUrl,
	            fileManagerJson:fileManagerJson,
	            allowFileManager : true,
	    		afterBlur:function(){
	    			this.sync();
		            var newhtml = this.html();
		            if(newhtml != defaultVal){
		                $(htmlID).trigger("change");
		            }
	    		}
		    });
		    // editor.sync();
	    });
	}

	/*
     * 创建分类选择弹层
     */
    P.createrCategoryTip = function(options){

		// 初始化配置
	    var configs = {
	    	cateSpan:"#cateBtn",											// 显示分类值 node ID
	    	cateInput:"#productCate",										// 存储分类值 node ID
	    	cateParamWrap:"#productCateParamWrap",							// 分类属性外围容器
	    	cateParam:"#productCateParam",									// 分类属性输出客器
	    	popupHtmlString:'<div class="m-category"><div class="title">请选择分类</div><div class="conter box"></div><div class="footer"><a href="javascript:;" class="ui-btn red">确定</a></div><a href="javascript:;" class="close">×</a></div>',			 // 弹层 Node ID
			popupCssFlag:"categoryPopFlag",									// CSS标识，以防重复加载
			popupCssLink:"http://base.ccdfs.cc/manager/css/public/m-category.css",	// 弹层CSS
			cateDataUrl:"http://api.ccdfs.cc/admin/cate.php",						// 分类数据JSON
			cateParamUrl:"http://api.ccdfs.cc/admin/cateParam.php"				// 分类属性数据JSON
	    }
	    configs = $.extend(configs, options);

	    if($(configs.cateSpan).hasClass("disabled")){
	    	return false;
	    }

		// step1 创建分类选择弹层
		function cateTip(opts){
			// 变量存储
			var level = 0;
			// 初始化 cate wrap
			function createrCateBox(node,arr,_level,type){
				// 数据转HTML
				var listHTML = '<li class="selected" value="">请选择</li>';
				if(arr.length>0){
					for (var i = 0 , len = arr.length; i < len; i++) {
						listHTML += '<li value="'+arr[i]['id']+'">'+arr[i]['name']+'</li>';
					};
				}
				// 创建select实体
				var HTML = '<ul>' + listHTML + '</ul>';
				if(type && type=="edit"){
					$(node).find("ul:eq("+_level+")").html(listHTML);
				}else{
					node.append(HTML);
					// 循环输出
					level++;
					if(level<3){
						createrCateBox(node,[],level);
					}
				}
			}

			// 初始化
			createrCateBox($(opts.node).find(".conter"),opts.data);

			// 一级分类事件
			$(opts.node).find("ul:eq(0)").on("click","li",function(){
				// 选中一级分类
				$(this).addClass("selected").siblings().removeClass("selected");
				// 生成二级分类
				var index = $(this).index();
				if(index>0 && opts.data[index-1]["categoryList"]){
					createrCateBox(opts.node,opts.data[index-1]["categoryList"],1,"edit");
				}else{
					createrCateBox(opts.node,[],1,"edit");
				}
				// 生成三级分类
				createrCateBox(opts.node,[],2,"edit");
			});

			// 二级分类事件
			$(opts.node).find("ul:eq(1)").on("click","li",function(){
				// 得到一级选中
				var lev0 = $(opts.node).find("ul:eq(0)").find(".selected").index();
				// 选中二级分类
				$(this).addClass("selected").siblings().removeClass("selected");
				// 生成三级分类
				var index = $(this).index();
				if(index>0 && opts.data[lev0-1]["categoryList"][index-1]["categoryList"]){
					createrCateBox(opts.node,opts.data[lev0-1]["categoryList"][index-1]["categoryList"],2,"edit");
				}else{
					createrCateBox(opts.node,[],2,"edit");
				}
			});

			// 二级分类事件
			$(opts.node).find("ul:eq(2)").on("click","li",function(){
				// 选中三级分类
				$(this).addClass("selected").siblings().removeClass("selected");
			});

			// 取值
			$(opts.node).on("click",".ui-btn",function(){
				// 取值
				var lev0 = $(opts.node).find("ul:eq(0)").find(".selected").text();
				var lev1 = $(opts.node).find("ul:eq(1)").find(".selected").text();
				var lev2 = $(opts.node).find("ul:eq(2)").find(".selected").text();
				// 去掉绑定事件
				$(opts.node).find("ul").off("click");
				opts.node.value = lev0 + ' > '+ lev1 + ' > '+ lev2 ;
				$(opts.node).find(".close").click();
			});
		}

		// step3 创建分类属性区
		function createrParamArea(data){
			var HTML = '<div class="paramList box">';
			for (var i = 0 , len = data.length; i < len; i++) {
				var keyHTML = data[i]['FontName'];
				var valHTML = T.createrFormElement({
					type:"checkbox",
					required:"true",
					name:"cateValue__"+i,
					model:{
						text:"SkuAttrValue",
						value:"SkuAttrValueID",
					},
					data:data[i]['Pro_SkuAttrbuteValueList'],
					customAttribute:["SkuAttrID","IsEdit","SortOrder","tanziliang"]
				});
				HTML += '<dl><dt>' + keyHTML +'：</dt><dd>' + valHTML + '</dd></dl>';
			};
			HTML += '</div>';
			$(configs.cateParam).html(HTML);
			// 注册验证
			T.registerFileds(configs.cateParam);
		}

		// step2 获得分类属性区数据
		function showCateParam(value){
			T.GET({
				action:configs.cateParamUrl,
				params:{v:value},
				success:function(data){
					// console.log(data.data.skuAttrData);
					$(configs.cateParamWrap).show();
					if(data.errno==0 && data.data.skuAttrData.length > 0){
						// 创建属性显示区
						createrParamArea(data.data.skuAttrData);
					}
				},
				error:function(o){
					console.log("T.error");
				}
			});
		}

		// 打开分类弹层
		T.popup({
			width:710,
			height:410,
			htmlString:configs.popupHtmlString,
			htmlCssFlag:configs.popupCssFlag,
			htmlCssLink:configs.popupCssLink,
			beforeShow:function(node){
				T.GET({
					action:configs.cateDataUrl,
					// params:{index:1,page:2},
					success:function(data){
						if(data.errno==0 && data.data.length > 0){
							// 创建弹层
							cateTip({
								node:node,
								data:data.data,
							});
						}
					},
					error:function(o){
						console.log("T.error");
					}
				});
			},
			afterShow:function(node){
				if(node.value){
					$(configs.cateSpan).find("em").html(node.value);
					$(configs.cateInput).val("fadsf");
					showCateParam(node.value);
				}
			}
		});

	}

	/*
     * 退回上一页
     */
	P.goBack = function(){
	    //if (document.referrer) {
	    //    window.location.href = document.referrer;
	    //} else {
	        window.history.back(-1);
	    //}
	}

})();




/* 
 * *************************************************************
 * 
 * 下面公共执行代码
 * 
 * *************************************************************
 */


$(function(){

	// $("a").on("click",function(){
	// 	var href = $(this).attr("href");
	// 	if(href != "#" )
	// }); 
	// 当前iframe打开的窗口地址
    if (!$("body").attr("popup") && window.parent.customFrame) {
		// console.log(frameElement.contentWindow.location.href);
		window.parent.customFrame.refresh(null,frameElement.contentWindow.location.href);
	}

	// 依赖 jquery.js or frame.js | document.domain 必须与父窗口相同
	// 关闭父级窗口中右键事件打开的操作项弹出层
	// 所有 iframe 子页都要加上这句代码避免报错
	// 没有 popup 属性，（表示弹出层iframe）
	if(window.parent.hideRightMenu){
		$(document).on("click",function(){
			window.parent.hideRightMenu();
		});
	}

	// 禁止F5刷新当前窗口 F5的window.event.keyCode==116
	$(window).off('keydown').on("keydown",function(e){
		var ev = window.event || e;
	    var code = ev.keyCode || ev.which;
	    // console.log(code);
	    if (code == 116) {
	        ev.keyCode ? ev.keyCode = 0 : ev.which = 0;
	        if (e && e.preventDefault){
	        	e.preventDefault(); 
	        }else{
				event.returnValue = false;
				window.event.cancelBubble=true;
			}
			return false;
	    }
	});

	// 为所有的TAB选项卡绑定
	$(".tabBox").tab();
	

});

