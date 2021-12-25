/**
 * select 	原生伪包装 rel: form-select.css
 * @authors 谭子良
 * @date    2014-05-27 13:49:01
 * @version 1.0.0
 */

;(function($, window, undefined) {
	$.fn.rightMenu = function(options) {
		// 默认参数
		var defaults = { 
			menuMode : "defaultModeName",	// 弹层ID名称 下面是对应的函数
			menuList: [
		        { menuName: '右键菜单1', clickEvent: function(o){alert(1)}},
		        { menuName: '右键菜单2', clickEvent: function(o){alert(2)}},
		        { menuName: 'split_line'},
		        { menuName: '右键菜单3', clickEvent: function(o){alert(3)}},
		        { menuName: '右键菜单4', clickEvent: function(o){alert(4)}},
		        { menuName: '右键菜单5', clickEvent: function(o){alert(5)}}
		    ],								// 添加菜单列表及回调
		    splitCss : "rightMenuSplit",	// 分隔线样式
		    menuBoxCss : 'rightMenu'		// 弹层样式名称
		};
		var opts = $.extend(defaults, options);
		// 随机ID
		var setTempID = function(){
			var d = new Date();
			var nRandom = (Math.random() * 1000000).toFixed(0);
			return Math.floor(d.getTime() + nRandom) ;
		}
		// 关闭弹层
		window.hideRightMenu = function(){
			$("."+opts.menuBoxCss).hide();
		}
		// 初始化
		var init = function(me){
			if($("#"+opts.menuMode+"").length > 0 ){
				// console.log("已经添加了");
			}else{
				var mHtml = '<div style="display:none;" id="'+opts.menuMode+'" class="'+opts.menuBoxCss+'"><ul>';
				$(opts.menuList).each(function(k,v){
					if(v.menuName=="split_line"){
						mHtml += '<li class="'+opts.splitCss+'"></li>';
					}else{
						mHtml += "<li>"+v.menuName+"</li>";
					}
				});
				mHtml += '</ul></div>';
				$("body").append(mHtml);
			}
			// 绑定右键事件
			me.oncontextmenu = function(){
				var e = arguments[0] || window.event;
				var x = e.pageX , y = e.pageY;
				$("#"+opts.menuMode).css({"left":x,"top":y}).show();

				// 相同的组共享一个相同的点击事件函数
				$("#"+opts.menuMode).off("click").on("click","li",function(){
					window.hideRightMenu();
					var index = $(this).index();
					opts.menuList[index].clickEvent(me);
				});

				return false;
			}
			$(document).on("click",function(){
				window.hideRightMenu();
			});

		};
		//绑定JQ循环
		return this.each(function(){
			init(this);
		});
	};
})(jQuery,window);

