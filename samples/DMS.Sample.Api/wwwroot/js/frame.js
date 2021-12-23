// Admin Pages Frame
// 依赖 jquery.js 
// 依赖 jquery.rightMenu.js 
// 依赖 js/public/tools.js
var customFrame = {
	// 配置参数
	opts:{
		isInitEnd:false,					// 初始化是否完成
		logo:"#logo",						// 用来切换宽窄屏显示
		currentCss:"current",				// 选中样式
		menuRoot:"#menuRoot",				// 顶级导航容器ID
		menuRootTpl:'#menuRootTpl',			// 顶级导航容器的模板ID
		menuSub:'#menuSub',					// 子类导航容器ID
		menuSubTpl:"#menuSubTpl",			// 子类导航容器的模板ID
		subItemCss:"box",					// 子类导航下面的子集样式名
		subHideCss:"hide",					// 子类导航折叠后样式名
		pagesWrap:"#mainWrap",				// 右侧主窗口区最外围ID
		pagesTab:"#tabsList",				// 窗口TAB区容器ID
		pagesWindow:"#pagesWindow",			// 窗口区容器ID
		pagesItemCss:"item",				// 窗口区页面子集样式名
		morePagesBtn:"#tabsMore",			// 更多窗口按钮
		morePagesTip:"tabsMoreTip",			// 更多窗口列表容器样式名
		morePagesCount:"count",				// 打开的页面总数统计 默认只有首页显示：1
		requires:{
			// 请求数据
			url:"json/menu.json",			// 请求地址
			para:{							// 传参
				pageIndex:1,
				productid:15320
			}
		},
		tabsListLeft:"tabsListLeft",		// 左侧可滚动时显示
		tabsListRight:"tabsListRight"		// 右侧可滚动时显示
	},
	// 窗口自适应函数
	winAuto : function(){
		$(this.opts.menuSub).height($(window).height()-50);
		$(this.opts.pagesWindow).height($(window).height()-103);
		if($(this.opts.menuSub).hasClass("un")){
			$(this.opts.pagesTab).width($(window).width()-112);
		}else{
			$(this.opts.pagesTab).width($(window).width()-262);
		}
		// 自动定位显示TABS
		if(this.opts.isInitEnd){
			var i = $(this.opts.pagesTab).find("li."+this.opts.currentCss).index();
			this.tabAuto(this.opts.pagesTab,i,true);
		}
	},
	// 定位 TABS 显示位置
	tabAuto:function(_tab,_cut,_target){
		var _len = $(_tab).find("li").length;
		if(_cut == _len-1){
			$('.'+this.opts.tabsListRight).css({"display":"none"});
		}else{
			$('.'+this.opts.tabsListRight).css({"display":""});
		}
		var _scLeft = $(_tab).scrollLeft();
		var _maxWidth = $(_tab).width() ;
		var _lastWidth = $(_tab).find("li").eq(_len-1).outerWidth();
		var _lastLeft = $(_tab).find("li").eq(_len-1).position().left;
		// 显示更多窗口下拉菜单
		if ( _scLeft + _lastLeft + _lastWidth + 5 > _maxWidth ){
			$(this.opts.morePagesBtn).css({"display":""});
		}else{
			$(this.opts.morePagesBtn).css({"display":"none"});
		}
		var _cutWidth = $(_tab).find("li").eq(_cut).outerWidth();
		var _cutLeft = $(_tab).find("li").eq(_cut).position().left;
		// scrollLeft 定位
		if(_cutLeft<0 || ( _cutWidth + 5 > _maxWidth - _cutLeft ) || _target == true){
			var toLeft = _cutLeft + _scLeft + _cutWidth + 5 - _maxWidth;
			if(toLeft<0){
				toLeft=0;
				$('.'+this.opts.tabsListLeft).hide();
			}else{
				$('.'+this.opts.tabsListLeft).show();
			}
			$(_tab).scrollLeft(toLeft);
		}
		// console.log(_cutLeft + _scLeft + _cutWidth + 5 - _maxWidth);
		// console.log( 'scrollLeft：' + _scLeft + '------position.left：' + _cutLeft + '------this.width：' + (_cutWidth + 5) + '------tabs.width：' + _maxWidth );
	},
	// 更多窗口显示时自动选中当前打开的窗口
	moreMenuAuto:function(){
		var node = $(this.opts.morePagesBtn);
		var index = node.find("li."+this.opts.currentCss).index();
		var top = node.find("li").eq(index).position().top;
		var scrTop = node.find("."+this.opts.morePagesTip).scrollTop();
		// console.log(top);
		if(top+scrTop>=391){
			var newScrTop = top+scrTop - 361;
			node.find("."+this.opts.morePagesTip).scrollTop(newScrTop);
		}
	},
	// 根据当前TAB次序显示层级关系
	updataRootPath:function(index){
		var me = this;
		var _index = index ? index : $(me.opts.pagesTab).find("li."+me.opts.currentCss).index();
		// 显示层次关系
    	if(_index==0){
    		// 系统首页
    		$(me.opts.menuSub).find("dd").removeClass(me.opts.currentCss);
    		$(me.opts.menuRoot).find("a").removeClass(me.opts.currentCss);
    		return;
    	}else{
			var cateid = $(me.opts.pagesTab).find("li").eq(_index).attr("id");
			var cateArr = cateid.split("_");
			$(me.opts.menuSub).find("dd").removeClass(me.opts.currentCss);
			var sIndex = $("#"+cateArr[0]+"_menu").addClass(me.opts.currentCss).closest("."+me.opts.subItemCss).index();
			$(me.opts.menuSub).find("."+me.opts.subItemCss).eq(sIndex).show().siblings().hide();
			$("#"+cateArr[1]+"_nav").addClass(me.opts.currentCss).siblings().removeClass(me.opts.currentCss);
	    }
	},
	// 高亮当前选中的选项卡及对应的内容
	switchPages:function(_index,_target){
		var me = this;
		var curTab = $(me.opts.pagesTab).find("li").eq(_index);
		var curSid = $(me.opts.morePagesBtn).find("li").eq(_index);
	    var items = $(me.opts.pagesWindow).children();
	    if( _target==='del'){
	    	if(_index==0){
	    		// 第一个不充许关闭
	    		return;
	    	}else{
	    		if(curTab.hasClass(me.opts.currentCss)){
					// 关闭当前
			    	curTab.prev().addClass(me.opts.currentCss).siblings().removeClass(me.opts.currentCss);
			    	curSid.prev().addClass(me.opts.currentCss).siblings().removeClass(me.opts.currentCss);
			    	items.eq(_index-1).show().siblings().hide();
			    	curTab.remove();
			    	curSid.remove();
			    	items.eq(_index).remove();
			    	me.tabAuto(me.opts.pagesTab,_index-1,true);
	    		}else{
	    			curTab.remove();
			    	curSid.remove();
			    	items.eq(_index).remove();
	    			var tsIndex = $(me.opts.pagesTab).find("li."+me.opts.currentCss).index();
	    			me.tabAuto(me.opts.pagesTab,tsIndex,true);
	    		}
	    		// 更新更多窗口列表数字
	    		var cut = $(me.opts.pagesTab).find("li").length;
	    		$(me.opts.morePagesBtn).find("."+me.opts.morePagesCount).html(cut);
	    	}
	    }else{
	    	// 选中当前高亮
			curTab.addClass(me.opts.currentCss).siblings().removeClass(me.opts.currentCss);
			curSid.addClass(me.opts.currentCss).siblings().removeClass(me.opts.currentCss);
			me.tabAuto(me.opts.pagesTab,_index);
			items.eq(_index).show().siblings().hide();
	    }
	    me.updataRootPath();
	},
	// 关闭页面 
	closePages:function(n,type){
		var me = this;
		var index = typeof(n)=="number"? n : $(n).index();
		var items = $(this.opts.pagesWindow).children();
		switch(type){
			case 'all':
				// 全部关闭			
				$(me.opts.pagesTab).find("li:not(:eq(0))").remove();
				$(me.opts.morePagesBtn).find("li:not(:eq(0))").remove();
				$(me.opts.pagesWindow).find(".item:not(:eq(0))").remove();
				$(me.opts.pagesTab).find("li:eq(0)").addClass(me.opts.currentCss);
				$(me.opts.pagesWindow).find(".item:eq(0)").show();
				me.tabAuto(me.opts.pagesTab,0,true);
				break;
			case 'notThis':
				// 自己除外全部关闭			
				$(me.opts.pagesTab).find("li:not(:eq(0),:eq("+index+"))").remove();
				$(me.opts.morePagesBtn).find("li:not(:eq(0),:eq("+index+"))").remove();
				$(me.opts.pagesWindow).find(".item:not(:eq(0),:eq("+index+"))").remove();
				$(me.opts.pagesTab).find("li:eq(1)").addClass(me.opts.currentCss);
				$(me.opts.pagesWindow).find(".item:eq(1)").show();
				me.tabAuto(me.opts.pagesTab,1,true);
				break;
			case 'before':
				// 当前显示左侧全部关闭			
				$(me.opts.pagesTab).find("li:lt("+index+"):gt(0)").remove();
				$(me.opts.morePagesBtn).find("li:lt("+index+"):gt(0)").remove();
				$(me.opts.pagesWindow).find(".item:lt("+index+"):gt(0)").remove();
				var i = $(this.opts.pagesTab).find("li."+this.opts.currentCss).index();
				if(i>0){
					me.tabAuto(me.opts.pagesTab,i,true);
				}else{
					$(me.opts.pagesTab).find("li:eq(1)").addClass(me.opts.currentCss);
					$(me.opts.pagesWindow).find(".item:eq(1)").show();
					me.tabAuto(me.opts.pagesTab,1,true);
				}
				break;
			case 'after':
				// 当前显示右侧全部关闭			
				$(me.opts.pagesTab).find("li:gt("+index+")").remove();
				$(me.opts.morePagesBtn).find("li:gt("+index+")").remove();
				$(me.opts.pagesWindow).find(".item:gt("+index+")").remove();
				var i = $(this.opts.pagesTab).find("li."+this.opts.currentCss).index();
				if(i>0){
					me.tabAuto(me.opts.pagesTab,i,true);
				}else{
					$(me.opts.pagesTab).find("li:eq("+index+")").addClass(me.opts.currentCss);
					$(me.opts.pagesWindow).find(".item:eq("+index+")").show();
					me.tabAuto(me.opts.pagesTab,index,true);
				}
				break;
			default:
				me.switchPages(index,"del");
				me.tabAuto(me.opts.pagesTab,index-1,true);
			;
		}
		me.updataRootPath();
	},
	// 刷新当前页面
	refresh:function(n,fromToIframe){
		if( !n && fromToIframe){
			// 来自子窗口数据
			// console.log(fromToIframe);
			var index = $(this.opts.pagesTab).find("li."+this.opts.currentCss).index();
			var items = $(this.opts.pagesWindow).children();
			var _iframe = items.eq(index).find("iframe");
				_iframe.attr("data-src",fromToIframe);
		}else{
			// 替换或添加地址中的 rand=1440121031601 
			var index = typeof(n)=="number"? n : $(n).index();
			var items = $(this.opts.pagesWindow).children();
			var _iframe = items.eq(index).find("iframe");
			var _src = _iframe.attr("data-src") ? _iframe.attr("data-src") : _iframe.attr("src");
			var _rand = "rand=" + (new Date().getTime());
			if( _src.indexOf("rand=") < 0 ){
				var _url = _src += (_src.indexOf('?') < 0 ? '?' : '&') + _rand;
				_iframe.attr("src",_url);
			}else{
				var _url = _src.replace(/(rand=)[0-9]{5,}/g,_rand);
				_iframe.attr("src",_url);
			}
		}
	},
	// 在新窗口打开当前页面
	openNewwindow:function(o){
		var index = $(o).index();
		var items = $(this.opts.pagesWindow).children();
		var url = items.eq(index).find("iframe").attr("src");
		window.open(url);
	},
	// 绑定事件
	binEvent : function(){
		// 当前对象
		var me = this;
		// 绑定窗口事件
		$(window).on("resize",function(){
			me.winAuto();					// 窗口自适应
		});
		// 禁止F5刷新当前窗口 F5的window.event.keyCode==116
		$(window).off('keydown').on("keydown",function(e){
			var ev = window.event || e;
		    var code = ev.keyCode || ev.which;
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
		// 切换宽窄屏
        $(me.opts.menuSub).off("click").on("click","h2",function(){
            if($(me.opts.menuSub).hasClass("un")){
            	$(me.opts.logo).removeClass("un");
                $(me.opts.menuSub).removeClass("un");
                $(me.opts.pagesWrap).removeClass("un");
                $(me.opts.menuSub).find("em").css({"display":"block"});
            }else{
            	$(me.opts.logo).addClass("un");
                $(me.opts.menuSub).addClass("un");
                $(me.opts.pagesWrap).addClass("un");
                $(me.opts.menuSub).find("em").css({"display":"none"});
                // 为动画效果而设
                window.setTimeout(function(){
                    $(me.opts.menuSub).find("em").css({"display":"block"});
                },1000);
            }
            me.winAuto();
        });
		// 切换顶级导航
		$(me.opts.menuRoot).on("click","a",function(){
			var index = $(this).index();
			if($(this).hasClass(me.opts.currentCss)){
				return;
			}else{
				$(this).addClass(me.opts.currentCss).siblings().removeClass(me.opts.currentCss);
				$(me.opts.menuSub).find("."+me.opts.subItemCss).eq(index).show().siblings().hide();
			}
		});
		// 折叠显示
		$(me.opts.menuSub).on("click","dt",function(){
			var wrap = $(this).closest("dl");
			if(wrap.hasClass(me.opts.subHideCss)){
				wrap.removeClass(me.opts.subHideCss);
			}else{
				wrap.addClass(me.opts.subHideCss);
			}
		});
		// 打开窗口
		$(me.opts.menuSub).on("click","dd",function(){
			$(me.opts.menuSub).find("dd").removeClass(me.opts.currentCss);
			$(this).addClass(me.opts.currentCss);
			var _url = $(this).attr("url");
			_url += (_url.indexOf('?') < 0 ? '?rand=' : '&rand=') + (new Date().getTime());
			var _name = $(this).text();
			var _id = parseInt($(this).attr("id"));
			var _pID = parseInt($(this).closest("."+me.opts.subItemCss).attr("id"));
			var isVing = $(me.opts.pagesTab).find("#"+_id+'_'+_pID);
			if(isVing && isVing.length>0){
				var isVingIndex = isVing.index();
				me.switchPages(isVingIndex,"b");
				$(me.opts.pagesWindow).find("."+me.opts.pagesItemCss).eq(isVingIndex).find("iframe").attr("src",_url);
				// me.refresh(isVingIndex);
				return;
			}else{
				$(me.opts.pagesTab).find("ul").append('<li id="'+_id+'_'+_pID+'"><b>'+_name+'</b><del>close</del></li>').find("li:last").addClass(me.opts.currentCss).siblings().removeClass(me.opts.currentCss);
				$(me.opts.morePagesBtn).find("ul").append('<li id="'+_id+'_'+_pID+'_side"><b>'+_name+'</b><del>close</del></li>').find("li:last").addClass(me.opts.currentCss).siblings().removeClass(me.opts.currentCss);
				$(me.opts.pagesWindow).append('<div class="'+me.opts.pagesItemCss+'"><iframe src="'+_url+'" frameborder="0" width="100%" height="100%" style="background:#fff;" scrolling="yes"></iframe></div>').find("."+me.opts.pagesItemCss+":last").show().siblings().hide();
				var cut = $(me.opts.pagesTab).find("li").length;
				$(me.opts.morePagesBtn).find("."+me.opts.morePagesCount).html(cut);
				me.tabAuto(me.opts.pagesTab,cut-1);
				if($.fn.rightMenu && typeof($.fn.rightMenu) == 'function'){
					$(me.opts.pagesTab).find("li").eq(cut-1).rightMenu({ 
						menuMode : "tabsRightMode",
						menuList: [
					        { menuName: '刷新', clickEvent: function(o){me.refresh(o);}},
					        { menuName: '在新窗口打开', clickEvent: function(o){me.openNewwindow(o)}},
					        { menuName: 'split_line'},
					        { menuName: '关闭', clickEvent: function(o){me.closePages(o)}},
					        { menuName: '全部关闭', clickEvent: function(o){me.closePages(o,"all")}},
					        { menuName: '除此之外全部关闭', clickEvent: function(o){me.closePages(o,"notThis")}},
					        { menuName: '当前右侧全部关闭', clickEvent: function(o){me.closePages(o,"after")}},
					        { menuName: '当前左侧全部关闭', clickEvent: function(o){me.closePages(o,"before")}}
					    ],
					    splitCss : 'rightMenuSplit',
					    menuBoxCss : 'rightMenu'
					});
				}else{
					console.log("error:rightMenu()");
				}
			}
		});
		// 切换窗口 与 关闭窗口
		$(me.opts.pagesTab).on("click","li",function(event){
			var index = $(this).index();
			var target = $(event.target).is("del") ? "del" : "b";
		    // 禁止冒泡
			if (event.stopPropagation) {
                event.stopPropagation();	//其它
                } else {
                event.cancelBubble = true;	//IE
            }
            me.switchPages(index,target);
		});
		// 更多窗口
		$(me.opts.morePagesBtn).on("click","li",function(event){
			var index = $(this).index();
			var target = $(event.target).is("del") ? "del" : "b";
		    // 禁止冒泡
			if (event.stopPropagation) {
                event.stopPropagation();	//其它
                } else {
                event.cancelBubble = true;	//IE
            }
            me.switchPages(index,target);
		});
		// 显示更多窗口
		$(me.opts.morePagesBtn).hover(function(){
			$(this).find("."+me.opts.morePagesTip).show();
			me.moreMenuAuto();
		},function(){
			$(this).find("."+me.opts.morePagesTip).hide();
		});
		// 刷新第一个TAB页
		if($.fn.rightMenu && typeof($.fn.rightMenu) == 'function'){
			// 刷新首页 将当前页面window对象传入子页供随时调
			me.refresh(0);
			$(me.opts.pagesTab).find("li").rightMenu({ 
				menuMode : "defaultRightMode",
				menuList: [
			        { menuName: '刷新', clickEvent: function(o){me.refresh(o);}},
			        { menuName: '在新窗口打开', clickEvent: function(o){me.openNewwindow(o)}},
			        { menuName: '除此之外全部关闭', clickEvent: function(){alert($().attr("id"))}}
			    ],
			    splitCss : 'rightMenuSplit',
			    menuBoxCss : 'rightMenu'
			});
		}else{
			console.log("error:rightMenu()");
		}
	},
	// 获取视窗数据
	getViewData:function(){
		// 请求传参
		var me = this;
		var opts = me.opts.requires;
		if (window.T) {
			T.GET({
				action:opts.url,
				params:opts.para,
				success: function(json){
					 alert("OK");
				    // var json = JSON.parse(json);
				    if (json.errno == 0) {
				        json.data.menuPool =JSON.parse( '[{"RightsID":1,"RightsName":"SysCenter","DisplayName":"系统中心","GroupRights":[{"RightsID":3,"RightsName":"Sys001","DisplayName":"系统维护","GroupRights":[{"RightsID":42,"RightsName":"MenuSysCenterList","DisplayName":"系统生成管理","URLAddr":"/SysBase/SysCenterList.aspx","URLName":"SysCenterList"},{"RightsID":4623,"RightsName":"MenuSysAdminOperationLogList","DisplayName":"操作日志","URLAddr":"/SysBase/SysAdminOperationLogList.aspx","URLName":"SysAdminOperationLogList.aspx"},{"RightsID":86,"RightsName":"MenuLogFileList","DisplayName":"文件日志","URLAddr":"/SysBase/LogFileList.aspx","URLName":"LogFileList"},{"RightsID":4625,"RightsName":"MenuSysErrorList","DisplayName":"错误日志","URLAddr":"/SysBase/SysErrorList.aspx","URLName":"SysErrorList.aspx"},{"RightsID":524,"RightsName":"MenuJobLogList","DisplayName":"服务日志","URLAddr":"/SysBase/LogJobList.aspx","URLName":"LogJobList.aspx"},{"RightsID":545,"RightsName":"MenuCustomsLogList","DisplayName":"海关日志","URLAddr":"/Customs/KeepOnRecordsOfCustomsLogList.aspx","URLName":"KeepOnRecordsOfCustomsLogList.aspx"},{"RightsID":4642,"RightsName":"MenuESLogList","DisplayName":"ES日志列表","URLAddr":"/SysBase/ESLogList.aspx","URLName":"ESLogList.aspx"},{"RightsID":4643,"RightsName":"MenuDictionaryList","DisplayName":"字典管理","URLAddr":"/SysBase/DicTypeList.aspx","URLName":"DicTypeList.aspx"}]},{"RightsID":2,"RightsName":"Sys002","DisplayName":"用户权限","GroupRights":[{"RightsID":5,"RightsName":"MenuRightsList","DisplayName":"菜单模块管理","URLAddr":"/SysBase/RightsList.aspx","URLName":"RightsList"},{"RightsID":25,"RightsName":"MenuDepList","DisplayName":"组织架构管理","URLAddr":"/SysBase/DeptList.aspx","URLName":"DeptList"},{"RightsID":27,"RightsName":"MenuGroupList","DisplayName":"职位列表管理","URLAddr":"/SysBase/GroupList.aspx","URLName":"GroupList"},{"RightsID":26,"RightsName":"MenuUserList","DisplayName":"后台用户管理","URLAddr":"/SysBase/UserList.aspx","URLName":"UserList"}]},{"RightsID":4629,"RightsName":"Sys003","DisplayName":"系统常用","GroupRights":[{"RightsID":4632,"RightsName":"MenuPayPayMerchantList","DisplayName":"支付方式管理","URLAddr":"/SysBase/PayPayMerchantList.aspx","URLName":"PayPayMerchantList.aspx"},{"RightsID":4634,"RightsName":"MenuSysPageAreaOptionList","DisplayName":"内容配置树管理","URLAddr":"/SysBase/SysPageAreaOptionList.aspx","URLName":"SysPageAreaOptionList.aspx"},{"RightsID":4636,"RightsName":"MenuSysAddressInfoList","DisplayName":"地址信息管理","URLAddr":"/SysBase/SysAddressInfoList.aspx","URLName":"SysAddressInfoList.aspx"},{"RightsID":4638,"RightsName":"MenuSysSEOTypeList","DisplayName":"SEO配置类型管理","URLAddr":"/SysBase/SysSEOTypeList.aspx","URLName":"SysSEOTypeList.aspx"},{"RightsID":4653,"RightsName":"MenuSysWorkQueueList","DisplayName":"邮件队列列表","URLAddr":"/SysBase/SysWorkQueueList.aspx","URLName":"SysWorkQueueList.aspx"},{"RightsID":4654,"RightsName":"MenuSysPrinterList","DisplayName":"打印配置列表","URLAddr":"/SysBase/SysPrinterList.aspx","URLName":"SysPrinterList.aspx"},{"RightsID":4655,"RightsName":"MenuSearchDictionaryList","DisplayName":"搜索词典管理","URLAddr":"/Sysbase/SearchDictionaryList.aspx","URLName":"SearchDictionaryList.aspx"},{"RightsID":4656,"RightsName":"MenuSysMemCacheList","DisplayName":"系统缓存列表","URLAddr":"/SysBase/SysMemCacheList.aspx","URLName":"SysMemCacheList.aspx"},{"RightsID":4657,"RightsName":"MenuLogisticsDistributionList","DisplayName":"物流配送管理","URLAddr":"/SysBase/LogisticsDistributionList.aspx","URLName":"LogisticsDistributionList.aspx"},{"RightsID":4658,"RightsName":"MenuSysAddressInfoFeeList","DisplayName":"运费配置管理","URLAddr":"/SysBase/SysAddressInfoFeeList.aspx","URLName":"SysAddressInfoFeeList.aspx"},{"RightsID":4659,"RightsName":"MenuNoticeServiceList","DisplayName":"通知服务列表","URLAddr":"/SysBase/NoticeServiceList.aspx","URLName":"NoticeServiceList.aspx"},{"RightsID":4660,"RightsName":"MenuSensitiveWordsList","DisplayName":"敏感词管理","URLAddr":"/SysBase/SensitiveWordsList.aspx","URLName":"SensitiveWordsList.aspx"},{"RightsID":4661,"RightsName":"MenuSysConfigList","DisplayName":"站点配置信息","URLAddr":"/Sysbase/SysConfigList.aspx","URLName":"SysConfigList.aspx"}]}]},{"RightsID":56,"RightsName":"MemberCenter","DisplayName":"会员中心","GroupRights":[{"RightsID":57,"RightsName":"Member001","DisplayName":"会员管理","GroupRights":[{"RightsID":58,"RightsName":"MenuMemberInfoList","DisplayName":"会员信息管理","URLAddr":"/Member/MemberAccountList.aspx","URLName":"MemberAccountList.aspx"},{"RightsID":64,"RightsName":"MenuReceiverAddressList","DisplayName":"收货地址管理","URLAddr":"/Member/MemberAddressList.aspx","URLName":"MemberAddressList"},{"RightsID":71,"RightsName":"MenuCollectionProductList","DisplayName":"会员收藏管理","URLAddr":"/Member/MemberFavoriteList.aspx","URLName":"MemberFavoriteList.aspx"}]},{"RightsID":497,"RightsName":"Member002","DisplayName":"会员咨询评论","GroupRights":[{"RightsID":501,"RightsName":"MenuProductConsultList","DisplayName":"商品咨询","URLAddr":"/Member/MemberCounselList.aspx","URLName":"MemberCounselList.aspx"},{"RightsID":502,"RightsName":"MenuProductCommentList","DisplayName":"商品评论","URLAddr":"/Member/MemberCommentList.aspx","URLName":"MemberCommentList.aspx"}]},{"RightsID":2631,"RightsName":"Member003","DisplayName":"会员营销","GroupRights":[{"RightsID":2634,"RightsName":"MenuUserIntegralList","DisplayName":"会员积分","URLAddr":"/Member/UserIntegralList.aspx","URLName":"UserIntegralList"},{"RightsID":2636,"RightsName":"UserWalletList","DisplayName":"会员钱包","URLAddr":"/Member/UserWalletList.aspx","URLName":"UserIntegralList"}]}]},{"RightsID":51,"RightsName":"ProductCenter","DisplayName":"商品中心","GroupRights":[{"RightsID":53,"RightsName":"Product001","DisplayName":"商品管理","GroupRights":[{"RightsID":54,"RightsName":"MenuProductCategoryList","DisplayName":"商品类别管理","URLAddr":"/Product/ProductCategoryList.aspx","URLName":"ProductCategoryList.aspx"},{"RightsID":221,"RightsName":"MenuPro_ProductModelList","DisplayName":"新增商品","URLAddr":"/Product/ProductModelList.aspx","URLName":"ProductModelList"},{"RightsID":75,"RightsName":"MenuGoodszhManagerList","DisplayName":"商品综合管理","URLAddr":"/product/ProductList.aspx","URLName":"ProductList"},{"RightsID":219,"RightsName":"MenuProReleasedList","DisplayName":"商品上架","URLAddr":"/Product/ProductReleasedList.aspx","URLName":"ProductReleasedList"},{"RightsID":527,"RightsName":"MenuKeepOnRecordsList","DisplayName":"商品备案管理","URLAddr":"/Customs/KeepOnRecordsOfCustomsList.aspx","URLName":"KeepOnRecordsOfCustomsList.aspx"}]},{"RightsID":510,"RightsName":"Product002","DisplayName":"基础数据","GroupRights":[{"RightsID":509,"RightsName":"MenuProductBrandList","DisplayName":"商品品牌管理","URLAddr":"/Product/ProductBrandList.aspx","URLName":"ProductBrandList"},{"RightsID":516,"RightsName":"MenuProductWarehouseList","DisplayName":"仓库信息管理","URLAddr":"/Product/ProductWarehouseList.aspx","URLName":"ProductWarehouseList"},{"RightsID":522,"RightsName":"MenuProductLogisticsList","DisplayName":"物流公司管理","URLAddr":"/Product/ProductLogisticsInfoList.aspx","URLName":"ProductLogisticsInfoList"},{"RightsID":616,"RightsName":"MenuTaxCategoryList","DisplayName":"行邮税管理","URLAddr":"/Customs/TaxCategoryList.aspx","URLName":"TaxCategoryList.aspx"}]}]},{"RightsID":7,"RightsName":"OrderCenter","DisplayName":"订单中心","GroupRights":[{"RightsID":16,"RightsName":"Order001","DisplayName":"订单管理","GroupRights":[{"RightsID":335,"RightsName":"MenuAllOrdersList","DisplayName":"全部订单","URLAddr":"/Order/Ord_AllOrderList.aspx?Action=All","URLName":"Ord_AllOrderList.aspx"},{"RightsID":582,"RightsName":"MenuWaitPayList","DisplayName":"未支付订单","URLAddr":"/Order/Ord_AllOrderList.aspx?Action=WaitPay","URLName":"Ord_AllOrderList.aspx"},{"RightsID":528,"RightsName":"MenuWaitOrderList","DisplayName":"待审核订单","URLAddr":"/Order/Ord_AllOrderList.aspx?Action=WaitCheckPass","URLName":"/Ord_AllOrderList.aspx"},{"RightsID":573,"RightsName":"MenuCustomsInsertFailList","DisplayName":"海关入库失败订单","URLAddr":"/Order/Ord_AllOrderList.aspx?Action=CustomsInsertFail","URLName":"/Ord_AllOrderList.aspx"},{"RightsID":576,"RightsName":"MenuCustomsNoPassList","DisplayName":"海关审核中与打回订单","URLAddr":"/Order/Ord_AllOrderList.aspx?Action=CustomsNoPass","URLName":"/Ord_AllOrderList.aspx"},{"RightsID":530,"RightsName":"MenuCustomsPassList","DisplayName":"手动提交信息到仓库","URLAddr":"/Order/Ord_AllOrderList.aspx?Action=CustomsPass","URLName":"/Ord_AllOrderList.aspx"},{"RightsID":578,"RightsName":"MenuWaitStoreOutList","DisplayName":"待发货订单","URLAddr":"/Order/Ord_AllOrderList.aspx?Action=WaitStoreOut","URLName":"/Ord_AllOrderList.aspx"},{"RightsID":580,"RightsName":"MenuOrderStoreOutList","DisplayName":"已发货未签收订单","URLAddr":"/Order/Ord_AllOrderList.aspx?Action=OrderStoreOut","URLName":"/Ord_AllOrderList.aspx"}]}]},{"RightsID":584,"RightsName":"MarketCenter","DisplayName":"营销中心","GroupRights":[{"RightsID":588,"RightsName":"Market001","DisplayName":"营销策略","GroupRights":[{"RightsID":589,"RightsName":"MenuDiscountList","DisplayName":"折扣","URLAddr":"/Market/DiscountList.aspx","URLName":"DiscountList.aspx"},{"RightsID":597,"RightsName":"MenuTimeLimitList","DisplayName":"限时","URLAddr":"/Market/TimeLimitList.aspx","URLName":"TimeLimitList.aspx"},{"RightsID":3622,"RightsName":"MenuOnTheHourSaleList","DisplayName":"整点抢","URLAddr":"/Market/OnTheHourSaleList.aspx","URLName":"OnTheHourSaleList.aspx"}]},{"RightsID":2637,"RightsName":"Market002","DisplayName":"页面数据管理","GroupRights":[{"RightsID":4621,"RightsName":"MenuPageAreaOptionList","DisplayName":"页面区域管理","URLAddr":"/Market/PageAreaOptionList.aspx","URLName":"PageAreaOptionList.aspx"},{"RightsID":2630,"RightsName":"MenuPageAreaConfigList","DisplayName":"页面配置管理","URLAddr":"/Market/PageAreaConfigList.aspx","URLName":"PageAreaConfigList.aspx"}]}]},{"RightsID":2627,"RightsName":"PortalCenter","DisplayName":"前台中心","GroupRights":[{"RightsID":2628,"RightsName":"Portal001","DisplayName":"前台配置","GroupRights":[{"RightsID":4670,"RightsName":"MenuSysPageAreaConfigList","DisplayName":"内容配置管理","URLAddr":"/SysBase/SysPageAreaConfigList.aspx?AreaType=0","URLName":"SysPageAreaConfigList.aspx"},{"RightsID":4680,"RightsName":"MenuSysPageAreaConfigList01","DisplayName":"首页配置管理","URLAddr":"/SysBase/SysPageAreaConfigList.aspx?AreaType=1","URLName":"SysPageAreaConfigList.aspx"},{"RightsID":4682,"RightsName":"MenuSysPageAreaConfigADAreaList","DisplayName":"广告配置管理","URLAddr":"/SysBase/SysPageAreaConfigADAreaList.aspx","URLName":"SysPageAreaConfigADAreaList.aspx"}]},{"RightsID":4683,"RightsName":"Portal002","DisplayName":"内容管理","GroupRights":[{"RightsID":4687,"RightsName":"MenuNoticeManager","DisplayName":"公告管理","URLAddr":"/CMS/NoticeList.aspx","URLName":"NoticeList.aspx"},{"RightsID":4688,"RightsName":"MenuMessageManager","DisplayName":"系统消息管理","URLAddr":"/CMS/MessageList.aspx","URLName":"MessageList.aspx"},{"RightsID":4689,"RightsName":"MenuSysSEOTableList","DisplayName":"SEO设置列表","URLAddr":"/SysBase/SysSEOTableList.aspx","URLName":"SysSEOTableList.aspx"},{"RightsID":4692,"RightsName":"MenuSysSEOTableCategoryList","DisplayName":"SEO设置列表-商品分类","URLAddr":"/SysBase/SysSEOTableCategoryList.aspx","URLName":"SysSEOTableCategoryList.aspx"},{"RightsID":4693,"RightsName":"MenuActivityTopicList","DisplayName":"专题活动列表","URLAddr":"/CMS/ActivityTopicList.aspx","URLName":"ActivityTopicList.aspx"}]},{"RightsID":4698,"RightsName":"Portal003","DisplayName":"数据统计","GroupRights":[{"RightsID":4704,"RightsName":"MenuSysSearchKeyList","DisplayName":"搜索关键词","URLAddr":"/SysBase/SysSearchKeyList.aspx","URLName":"SysSearchKeyList.aspx"},{"RightsID":4705,"RightsName":"MenuEMDInfoList","DisplayName":"EDM管理列表","URLAddr":"/CMS/EMDInfoList.aspx","URLName":"EMDInfoList.aspx"},{"RightsID":4707,"RightsName":"MenuSysVisitLogList","DisplayName":"前台历史日志","URLAddr":"/SysBase/SysVisitLogAllList.aspx?vtype=2","URLName":"SysVisitLogList.aspx"}]}]}]');
				        // 一级菜单
				        $(me.opts.menuRoot).empty();
				        $(me.opts.menuRootTpl).tmpl(json.data.menuPool).appendTo(me.opts.menuRoot);
				        $(me.opts.menuRoot).find("a").eq(0).addClass(me.opts.currentCss);
				        // 二级菜单
				        $(me.opts.menuSub).empty();
				        $(me.opts.menuSubTpl).tmpl(json.data.menuPool).appendTo(me.opts.menuSub);
				        $(me.opts.menuSub).find("." + me.opts.subItemCss).eq(0).show();
				        // 初始化视窗事件
				        me.opts.isInitEnd = true;
				        // 事件绑定
				        me.binEvent();
				    } else {
				        alert(json.errmsg);
				        window.location = "/login.html";
				    }
				},
				failure: function (res) {
				    if (res.errno == 3) {
				        alert(res.errmsg);
				        window.location = "/";
				    } else {
				        alert("系统逻辑错误");
				    }
				},
				error:function(e){
				    alert("error:T.GET");

				    var menuPool = JSON.parse('[{"RightsID":1,"RightsName":"SysCenter","DisplayName":"系统中心","GroupRights":[{"RightsID":3,"RightsName":"Sys001","DisplayName":"系统维护","GroupRights":[{"RightsID":42,"RightsName":"MenuSysCenterList","DisplayName":"系统生成管理","URLAddr":"/SysBase/SysCenterList.aspx","URLName":"SysCenterList"},{"RightsID":4623,"RightsName":"MenuSysAdminOperationLogList","DisplayName":"操作日志","URLAddr":"/SysBase/SysAdminOperationLogList.aspx","URLName":"SysAdminOperationLogList.aspx"},{"RightsID":86,"RightsName":"MenuLogFileList","DisplayName":"文件日志","URLAddr":"/SysBase/LogFileList.aspx","URLName":"LogFileList"},{"RightsID":4625,"RightsName":"MenuSysErrorList","DisplayName":"错误日志","URLAddr":"/SysBase/SysErrorList.aspx","URLName":"SysErrorList.aspx"},{"RightsID":524,"RightsName":"MenuJobLogList","DisplayName":"服务日志","URLAddr":"/SysBase/LogJobList.aspx","URLName":"LogJobList.aspx"},{"RightsID":545,"RightsName":"MenuCustomsLogList","DisplayName":"海关日志","URLAddr":"/Customs/KeepOnRecordsOfCustomsLogList.aspx","URLName":"KeepOnRecordsOfCustomsLogList.aspx"},{"RightsID":4642,"RightsName":"MenuESLogList","DisplayName":"ES日志列表","URLAddr":"/SysBase/ESLogList.aspx","URLName":"ESLogList.aspx"},{"RightsID":4643,"RightsName":"MenuDictionaryList","DisplayName":"字典管理","URLAddr":"/SysBase/DicTypeList.aspx","URLName":"DicTypeList.aspx"}]},{"RightsID":2,"RightsName":"Sys002","DisplayName":"用户权限","GroupRights":[{"RightsID":5,"RightsName":"MenuRightsList","DisplayName":"菜单模块管理","URLAddr":"/SysBase/RightsList.aspx","URLName":"RightsList"},{"RightsID":25,"RightsName":"MenuDepList","DisplayName":"组织架构管理","URLAddr":"/SysBase/DeptList.aspx","URLName":"DeptList"},{"RightsID":27,"RightsName":"MenuGroupList","DisplayName":"职位列表管理","URLAddr":"/SysBase/GroupList.aspx","URLName":"GroupList"},{"RightsID":26,"RightsName":"MenuUserList","DisplayName":"后台用户管理","URLAddr":"/SysBase/UserList.aspx","URLName":"UserList"}]},{"RightsID":4629,"RightsName":"Sys003","DisplayName":"系统常用","GroupRights":[{"RightsID":4632,"RightsName":"MenuPayPayMerchantList","DisplayName":"支付方式管理","URLAddr":"/SysBase/PayPayMerchantList.aspx","URLName":"PayPayMerchantList.aspx"},{"RightsID":4634,"RightsName":"MenuSysPageAreaOptionList","DisplayName":"内容配置树管理","URLAddr":"/SysBase/SysPageAreaOptionList.aspx","URLName":"SysPageAreaOptionList.aspx"},{"RightsID":4636,"RightsName":"MenuSysAddressInfoList","DisplayName":"地址信息管理","URLAddr":"/SysBase/SysAddressInfoList.aspx","URLName":"SysAddressInfoList.aspx"},{"RightsID":4638,"RightsName":"MenuSysSEOTypeList","DisplayName":"SEO配置类型管理","URLAddr":"/SysBase/SysSEOTypeList.aspx","URLName":"SysSEOTypeList.aspx"},{"RightsID":4653,"RightsName":"MenuSysWorkQueueList","DisplayName":"邮件队列列表","URLAddr":"/SysBase/SysWorkQueueList.aspx","URLName":"SysWorkQueueList.aspx"},{"RightsID":4654,"RightsName":"MenuSysPrinterList","DisplayName":"打印配置列表","URLAddr":"/SysBase/SysPrinterList.aspx","URLName":"SysPrinterList.aspx"},{"RightsID":4655,"RightsName":"MenuSearchDictionaryList","DisplayName":"搜索词典管理","URLAddr":"/Sysbase/SearchDictionaryList.aspx","URLName":"SearchDictionaryList.aspx"},{"RightsID":4656,"RightsName":"MenuSysMemCacheList","DisplayName":"系统缓存列表","URLAddr":"/SysBase/SysMemCacheList.aspx","URLName":"SysMemCacheList.aspx"},{"RightsID":4657,"RightsName":"MenuLogisticsDistributionList","DisplayName":"物流配送管理","URLAddr":"/SysBase/LogisticsDistributionList.aspx","URLName":"LogisticsDistributionList.aspx"},{"RightsID":4658,"RightsName":"MenuSysAddressInfoFeeList","DisplayName":"运费配置管理","URLAddr":"/SysBase/SysAddressInfoFeeList.aspx","URLName":"SysAddressInfoFeeList.aspx"},{"RightsID":4659,"RightsName":"MenuNoticeServiceList","DisplayName":"通知服务列表","URLAddr":"/SysBase/NoticeServiceList.aspx","URLName":"NoticeServiceList.aspx"},{"RightsID":4660,"RightsName":"MenuSensitiveWordsList","DisplayName":"敏感词管理","URLAddr":"/SysBase/SensitiveWordsList.aspx","URLName":"SensitiveWordsList.aspx"},{"RightsID":4661,"RightsName":"MenuSysConfigList","DisplayName":"站点配置信息","URLAddr":"/Sysbase/SysConfigList.aspx","URLName":"SysConfigList.aspx"}]}]},{"RightsID":56,"RightsName":"MemberCenter","DisplayName":"会员中心","GroupRights":[{"RightsID":57,"RightsName":"Member001","DisplayName":"会员管理","GroupRights":[{"RightsID":58,"RightsName":"MenuMemberInfoList","DisplayName":"会员信息管理","URLAddr":"/Member/MemberAccountList.aspx","URLName":"MemberAccountList.aspx"},{"RightsID":64,"RightsName":"MenuReceiverAddressList","DisplayName":"收货地址管理","URLAddr":"/Member/MemberAddressList.aspx","URLName":"MemberAddressList"},{"RightsID":71,"RightsName":"MenuCollectionProductList","DisplayName":"会员收藏管理","URLAddr":"/Member/MemberFavoriteList.aspx","URLName":"MemberFavoriteList.aspx"}]},{"RightsID":497,"RightsName":"Member002","DisplayName":"会员咨询评论","GroupRights":[{"RightsID":501,"RightsName":"MenuProductConsultList","DisplayName":"商品咨询","URLAddr":"/Member/MemberCounselList.aspx","URLName":"MemberCounselList.aspx"},{"RightsID":502,"RightsName":"MenuProductCommentList","DisplayName":"商品评论","URLAddr":"/Member/MemberCommentList.aspx","URLName":"MemberCommentList.aspx"}]},{"RightsID":2631,"RightsName":"Member003","DisplayName":"会员营销","GroupRights":[{"RightsID":2634,"RightsName":"MenuUserIntegralList","DisplayName":"会员积分","URLAddr":"/Member/UserIntegralList.aspx","URLName":"UserIntegralList"},{"RightsID":2636,"RightsName":"UserWalletList","DisplayName":"会员钱包","URLAddr":"/Member/UserWalletList.aspx","URLName":"UserIntegralList"}]}]},{"RightsID":51,"RightsName":"ProductCenter","DisplayName":"商品中心","GroupRights":[{"RightsID":53,"RightsName":"Product001","DisplayName":"商品管理","GroupRights":[{"RightsID":54,"RightsName":"MenuProductCategoryList","DisplayName":"商品类别管理","URLAddr":"/Product/ProductCategoryList.aspx","URLName":"ProductCategoryList.aspx"},{"RightsID":221,"RightsName":"MenuPro_ProductModelList","DisplayName":"新增商品","URLAddr":"/Product/ProductModelList.aspx","URLName":"ProductModelList"},{"RightsID":75,"RightsName":"MenuGoodszhManagerList","DisplayName":"商品综合管理","URLAddr":"/product/ProductList.aspx","URLName":"ProductList"},{"RightsID":219,"RightsName":"MenuProReleasedList","DisplayName":"商品上架","URLAddr":"/Product/ProductReleasedList.aspx","URLName":"ProductReleasedList"},{"RightsID":527,"RightsName":"MenuKeepOnRecordsList","DisplayName":"商品备案管理","URLAddr":"/Customs/KeepOnRecordsOfCustomsList.aspx","URLName":"KeepOnRecordsOfCustomsList.aspx"}]},{"RightsID":510,"RightsName":"Product002","DisplayName":"基础数据","GroupRights":[{"RightsID":509,"RightsName":"MenuProductBrandList","DisplayName":"商品品牌管理","URLAddr":"/Product/ProductBrandList.aspx","URLName":"ProductBrandList"},{"RightsID":516,"RightsName":"MenuProductWarehouseList","DisplayName":"仓库信息管理","URLAddr":"/Product/ProductWarehouseList.aspx","URLName":"ProductWarehouseList"},{"RightsID":522,"RightsName":"MenuProductLogisticsList","DisplayName":"物流公司管理","URLAddr":"/Product/ProductLogisticsInfoList.aspx","URLName":"ProductLogisticsInfoList"},{"RightsID":616,"RightsName":"MenuTaxCategoryList","DisplayName":"行邮税管理","URLAddr":"/Customs/TaxCategoryList.aspx","URLName":"TaxCategoryList.aspx"}]}]},{"RightsID":7,"RightsName":"OrderCenter","DisplayName":"订单中心","GroupRights":[{"RightsID":16,"RightsName":"Order001","DisplayName":"订单管理","GroupRights":[{"RightsID":335,"RightsName":"MenuAllOrdersList","DisplayName":"全部订单","URLAddr":"/Order/Ord_AllOrderList.aspx?Action=All","URLName":"Ord_AllOrderList.aspx"},{"RightsID":582,"RightsName":"MenuWaitPayList","DisplayName":"未支付订单","URLAddr":"/Order/Ord_AllOrderList.aspx?Action=WaitPay","URLName":"Ord_AllOrderList.aspx"},{"RightsID":528,"RightsName":"MenuWaitOrderList","DisplayName":"待审核订单","URLAddr":"/Order/Ord_AllOrderList.aspx?Action=WaitCheckPass","URLName":"/Ord_AllOrderList.aspx"},{"RightsID":573,"RightsName":"MenuCustomsInsertFailList","DisplayName":"海关入库失败订单","URLAddr":"/Order/Ord_AllOrderList.aspx?Action=CustomsInsertFail","URLName":"/Ord_AllOrderList.aspx"},{"RightsID":576,"RightsName":"MenuCustomsNoPassList","DisplayName":"海关审核中与打回订单","URLAddr":"/Order/Ord_AllOrderList.aspx?Action=CustomsNoPass","URLName":"/Ord_AllOrderList.aspx"},{"RightsID":530,"RightsName":"MenuCustomsPassList","DisplayName":"手动提交信息到仓库","URLAddr":"/Order/Ord_AllOrderList.aspx?Action=CustomsPass","URLName":"/Ord_AllOrderList.aspx"},{"RightsID":578,"RightsName":"MenuWaitStoreOutList","DisplayName":"待发货订单","URLAddr":"/Order/Ord_AllOrderList.aspx?Action=WaitStoreOut","URLName":"/Ord_AllOrderList.aspx"},{"RightsID":580,"RightsName":"MenuOrderStoreOutList","DisplayName":"已发货未签收订单","URLAddr":"/Order/Ord_AllOrderList.aspx?Action=OrderStoreOut","URLName":"/Ord_AllOrderList.aspx"}]}]},{"RightsID":584,"RightsName":"MarketCenter","DisplayName":"营销中心","GroupRights":[{"RightsID":588,"RightsName":"Market001","DisplayName":"营销策略","GroupRights":[{"RightsID":589,"RightsName":"MenuDiscountList","DisplayName":"折扣","URLAddr":"/Market/DiscountList.aspx","URLName":"DiscountList.aspx"},{"RightsID":597,"RightsName":"MenuTimeLimitList","DisplayName":"限时","URLAddr":"/Market/TimeLimitList.aspx","URLName":"TimeLimitList.aspx"},{"RightsID":3622,"RightsName":"MenuOnTheHourSaleList","DisplayName":"整点抢","URLAddr":"/Market/OnTheHourSaleList.aspx","URLName":"OnTheHourSaleList.aspx"}]},{"RightsID":2637,"RightsName":"Market002","DisplayName":"页面数据管理","GroupRights":[{"RightsID":4621,"RightsName":"MenuPageAreaOptionList","DisplayName":"页面区域管理","URLAddr":"/Market/PageAreaOptionList.aspx","URLName":"PageAreaOptionList.aspx"},{"RightsID":2630,"RightsName":"MenuPageAreaConfigList","DisplayName":"页面配置管理","URLAddr":"/Market/PageAreaConfigList.aspx","URLName":"PageAreaConfigList.aspx"}]}]},{"RightsID":2627,"RightsName":"PortalCenter","DisplayName":"前台中心","GroupRights":[{"RightsID":2628,"RightsName":"Portal001","DisplayName":"前台配置","GroupRights":[{"RightsID":4670,"RightsName":"MenuSysPageAreaConfigList","DisplayName":"内容配置管理","URLAddr":"/SysBase/SysPageAreaConfigList.aspx?AreaType=0","URLName":"SysPageAreaConfigList.aspx"},{"RightsID":4680,"RightsName":"MenuSysPageAreaConfigList01","DisplayName":"首页配置管理","URLAddr":"/SysBase/SysPageAreaConfigList.aspx?AreaType=1","URLName":"SysPageAreaConfigList.aspx"},{"RightsID":4682,"RightsName":"MenuSysPageAreaConfigADAreaList","DisplayName":"广告配置管理","URLAddr":"/SysBase/SysPageAreaConfigADAreaList.aspx","URLName":"SysPageAreaConfigADAreaList.aspx"}]},{"RightsID":4683,"RightsName":"Portal002","DisplayName":"内容管理","GroupRights":[{"RightsID":4687,"RightsName":"MenuNoticeManager","DisplayName":"公告管理","URLAddr":"/CMS/NoticeList.aspx","URLName":"NoticeList.aspx"},{"RightsID":4688,"RightsName":"MenuMessageManager","DisplayName":"系统消息管理","URLAddr":"/CMS/MessageList.aspx","URLName":"MessageList.aspx"},{"RightsID":4689,"RightsName":"MenuSysSEOTableList","DisplayName":"SEO设置列表","URLAddr":"/SysBase/SysSEOTableList.aspx","URLName":"SysSEOTableList.aspx"},{"RightsID":4692,"RightsName":"MenuSysSEOTableCategoryList","DisplayName":"SEO设置列表-商品分类","URLAddr":"/SysBase/SysSEOTableCategoryList.aspx","URLName":"SysSEOTableCategoryList.aspx"},{"RightsID":4693,"RightsName":"MenuActivityTopicList","DisplayName":"专题活动列表","URLAddr":"/CMS/ActivityTopicList.aspx","URLName":"ActivityTopicList.aspx"}]},{"RightsID":4698,"RightsName":"Portal003","DisplayName":"数据统计","GroupRights":[{"RightsID":4704,"RightsName":"MenuSysSearchKeyList","DisplayName":"搜索关键词","URLAddr":"/SysBase/SysSearchKeyList.aspx","URLName":"SysSearchKeyList.aspx"},{"RightsID":4705,"RightsName":"MenuEMDInfoList","DisplayName":"EDM管理列表","URLAddr":"/CMS/EMDInfoList.aspx","URLName":"EMDInfoList.aspx"},{"RightsID":4707,"RightsName":"MenuSysVisitLogList","DisplayName":"前台历史日志","URLAddr":"/SysBase/SysVisitLogAllList.aspx?vtype=2","URLName":"SysVisitLogList.aspx"}]}]}]');
				    // 一级菜单
				    $(me.opts.menuRoot).empty();
				    $(me.opts.menuRootTpl).tmpl(menuPool).appendTo(me.opts.menuRoot);
				    $(me.opts.menuRoot).find("a").eq(0).addClass(me.opts.currentCss);
				    // 二级菜单
				    $(me.opts.menuSub).empty();
				    $(me.opts.menuSubTpl).tmpl(menuPool).appendTo(me.opts.menuSub);
				    $(me.opts.menuSub).find("." + me.opts.subItemCss).eq(0).show();
				    // 初始化视窗事件
				    me.opts.isInitEnd = true;
				    // 事件绑定
				    me.binEvent();
				}
			});
		}
	},
	// 初始化
	init:function(options){
		this.opts = $.extend(this.opts, options);
		this.winAuto();								// 窗口自适应
		this.getViewData();							// 初始化视窗显示
	}
}

