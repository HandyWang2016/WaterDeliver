# WaterDeliver 1.0
水递夫桶装水管理
系统架构：asp.net MVC+MongoDB,前台框架Bootstrap+jquery.

本系统支持如下功能：
一.后台
1.客户管理
2.员工管理
3.产品管理
4.员工--客户关系管理
5.公司消费支出类型管理

二.前台
1.日常送水记录
2.日常纪录查看
3.月底统计日常送水信息
4.公司支出费用统计
5.公司支出查看
6.月底统计公司支出信息

开发周期：2017-12-16 ~ 2018-1-3
=======================================2018.1.3===============================================
#WaterDeliver 2.0
昨晚和姐夫交流后发现一些需要完善与改进的地方，统计如下：
一.后台
1.产品管理
1>同一款产品有可能进价不一样，如同是娃哈哈，进价有可能5元或6元，可以录入娃哈哈，娃哈哈2来解决，无需改动
2>添加字段：空桶库存，空桶成本
  之前的成本为桶装水的成本，空桶成本在计算厂家押金支出、厂家押金收入时用到
  每日一记中，送出桶装水的数量(品牌相关)要实时更新相应品牌的库存；回空桶的数量也要更新空桶库存
3>同一款产品售价也可能不同，但不重要，因为计算收益方法是：月底结算/出售水卡-桶装水成本，售价字段暂时保留
2.员工-客户关系管理
  考虑实际情况，暂时不再维护，保留
3.添加页面：
1>供应商管理
  供应商即厂家，不同品牌的水可能从不同厂家进，用处：产品与厂家关联;厂家押金支出与厂家关联;厂家押金收入与厂家关联
2>库存管理
  产品管理页面初始化第一次进货库存，之后该产品的库存不足时，来库存管理页面补充库存(选择供应商，选择品牌，添加数量，产品管理页面库存实时更新)
  
二.前台
1.每日一记
1>添加成本字段，当页面选择产品后，该字段实时显示该产品对应的成本，因为同一品牌有不同成本，便于记录时查看
2>送水桶数、收回空桶，都是对应选择的产品，保存后相应产品的水库存，空桶库存相应变动
2.月底结算
  添加送水成本、合计盈利字段：统计所送水的总成本，根据成本/押金/月底结算/水卡的费用，综合算出该月的初步盈利(与公司支出信息整合算出最终盈利)
3.公司支出
  添加供货商、产品选择字段，默认空，相关消费类(如进水支出、押金支出等)可以选择供货商与产品
4.添加页面
1>月底盈利
  根据成本/押金/月底结算/水卡的费用，综合算出该月的初步盈利,与公司支出信息整合算出最终盈利,相关字段展示，细节待定
2>押金退回
  可能和某一供货商停止合作，支付其的押金需要退回，算作公司收益，在该页面实现功能
  
============================================2018.1.4====================================================

#WaterDeliver 2.1
基于周日晚上与姐夫的沟通，系统有如下问题或需要改善的地方
1.后台不再做桶装水库存的维护
2.后台产品维护，添加如下产品：饮水机、水架、手压泵，并在前台每日一记添加维护
3.菜单名称做调整，能够更准确简单的定义每个菜单的功能
4.月底收支汇总页面，只保留收入、支出，取消员工收入员工支出，引起歧义
5.月底收支汇总页面,发工资模块，选择员工后显示该员工当月的提成

============================================2018.1.16====================================================

#WaterDeliver 2.1.1
基于这两次与姐夫的交流，系统做如下改动
1.进水支出参与公司盈利运算--程序修改纪录显示2018.1.9做出修改:进水支出不参与公司盈利运算，但具体原因没有记录，现在改回去
2.每日一记--选择客户/选择产品下拉框改为支出模糊搜索匹配
3.每日一记--添加附属产品：添加三种产品回收的输入框，回收信息计入日常纪录并做展示
4.后台统计每名送水员每天送水总成本，在日常记录中做统计(针对垫资的情况，垫付的钱在月底结算时返还给垫付人)
注：每次改动前记得写清楚改动内容，如有必要写清改动原因，避免以后无处可查或改动不全

============================================2018.3.13====================================================

#WaterDeliver 2.1.2
基于最近交流中系统遇到的问题与不完善之处，做如下改动
1.添加各产品送水成本统计功能---针对月中私人垫资买水的情况，月底根据该页面数据从收益中提取所垫资水产品的款额
2.日常记录：各个子模块添加导出功能
3.日常记录：公司查询条件改为可以模糊搜索
4.后台产品管理：删除失败bug修改；添加产品时产品名称重复性校验；删除产品后该产品已经录入的日常记录也删除

============================================2018.5.2====================================================



