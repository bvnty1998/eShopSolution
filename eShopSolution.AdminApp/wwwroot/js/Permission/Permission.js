var eShopPermission = {
    Init: function () {
        var caheObj = {
            allRole:[]
        }
        // Begin Utilities
            GetAllRole()
        // ENd Utilities

        // Begin Load Permission assgined
        $('#UserId').on('change', function () {
            /*console.log($(' input#11980688-84ee-462e-c50f-08d917a05fad.role.ROLE'))*/
            var id = $('#UserId').val()
            $.ajax({
                type: 'GET',
                data: {
                    id:id
                },
                url: '/Role/GetRoleById',
                success: function (jsonData) {
                    if (jsonData.success == true) {
                       
                        $.each(jsonData.data, function (i, roleChecked) {
                            $.each(caheObj.allRole, function (i, role) {
                                if (role.functionName == roleChecked.functionName && role.Name == roleChecked.roleName) {
                                    var id = 'input' + '#' + roleChecked.roleId + '.role.'+ roleChecked.functionName ;
                                  
                                    $(id).prop("checked", true);
                                }
                            })
                        })
                    }
                },
                erorr: function (jsonData) {
                    console.log(jsonData)
                }
            })
        })

        function GetAllRole() {
            var arrRole = [];
            var row = $('tr'); // lấy tất cả nhữn dòng trong bảng
            row.each(function (i, item) { // lặp từng dòng

                var coloumn = $(item).find('td')
                var functionName = coloumn.find('label.functionName');// lấy tên và id function
                var Roles = coloumn.find('input.role'); // lấy quyền của phần function
                if (Roles.length > 0) { // điều kiện > 0 để loại dòng tiêu để
                    // lặp để lấy tất cả các quyền
                    Roles.each(function (j, role) {
                        var role = {
                            functionName: functionName.text(),
                            functionId: functionName[0].id,
                            Id: role.id,
                            Name: role.value,
                            Description: ''
                        }
                        arrRole.push(role);
                    })
                }
            })
            caheObj.allRole = arrRole
        }
        // End Load Permission assgined
    }
   
}