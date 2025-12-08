    //< div class= "card container" >
    //    < div class= "d-flex justify-content-between" >
    //        < div class= "stat p-2" >
    //            < h2 > Create Teacher </ h2 >
    //            < span onclick = "document.getElementById('CreateTeacher').style.display='block'" class= "justify-content-center" >
    //                < button class= "btn" > Create Teacher </ button >
    //            </ span >
    //            < div class= "modal text-center" id = "CreateTeacher" >
    //                < div class= "modal-content" display = "none" >
    //                    < span onclick = "document.getElementById('CreateTeacher').style.display='none'" class= "btn-close display-topright" ></ span >
    //                    < h1 > Create Teacher </ h1 >
    //                    < form method = "post" asp - page - handler = "CreateTeacher" >
    //                        < p >< b > Institution: </ b > < br />
    //                            < label for= "Institution Selection" ></ label >
    //                            < select id = "InstitutionID" name = "InstitutionID" required >
    //                                @foreach(Zealand_LoMaS_Lib.Model.Institution institution in Model.Institutions)
    //                                {
    //                                    < option value = "@institution.InstitutionID" > @institution.Location.City </ option >
    //                                }
    //                            </ select >
    //                        </ p >
    //                        < p class= "" > < b > First Name: </ b > < br />< input type = "text" asp -for= "FirstName" name = "FirstName" placeholder = "First Name" required /></ p >
    //                        < p > < b > Last Name: </ b > < br />< input type = "text" asp -for= "LastName" name = "LastName" placeholder = "Last Name" required /></ p >
    //                        < p > < b > Email: </ b > < br />< input type = "email" asp -for= "Email" name = "Email" placeholder = "Email" required /></ p >
    //                        < p > < b > Hours: </ b > < br />< input type = "text" asp -for= "WeeklyHours" name = "WeeklyHours" placeholder = "Weekly Hours" required /></ p >
    //                        < p > < b > Has Car: </ b > < br />< input type = "text" name = "HasCar" asp -for= "HasCar" value = "true/false" required /> </ p >
    //                        < p >< b > Address </ b > < br />
    //                            < label for= "Location Selection" ></ label >
    //                            < select id = "Location" name = "Location" required >
    //                                @foreach(var institution in Model.Institutions)
    //                                {
    //                                    < option value = "@institution.InstitutionID" > @institution.Location.City </ option >
    //                                }
    //                            </ select >
    //                        </ p >
    //                        < input type = "hidden" name = "AdminIDs" asp -for= "AdminIDs" value = "@Convert.ToInt32(HttpContext.Request.Cookies["UserID"])" required />
    //                        < input type = "submit" value = "Create" />
    //                    </ form >
    //                </ div >
    //            </ div >
    //        </ div >
    //        < div class= "stat p-2" >

    //        </ div >
    //        < div class= "stat p-2" >

    //        </ div >
    //    </ div >
    //</ div >
    //< h1 > Teachers </ h1 >
    //< table class= "table" >
    //    < tbody >
    //        < tr >
    //            < th > Institution </ th >
    //            < th > Name </ th >
    //            < th > Email </ th >
    //            < th > Weekly Hours </ th >
    //            < th > Car </ th >
    //            < th > Address </ th >
    //            < th > Admins </ th >
    //            < th > Edit </ th >
    //            < th > Remove </ th >
    //        </ tr >
    //        @foreach(var T in Model.Teachers)
    //        {
    //            < tr >
    //                < td >
    //                    @foreach(var I in Model.Institutions)
    //                    {
    //    if (I.InstitutionID == T.InstitutionID)
    //    {
    //                            < p > @I.Location.RoadName @I.Location.RoadNumber, @I.Location.City </ p >
    //                        }
    //}
    //                </ td >
    //                < td > @T.FirstName @T.LastName </ td >
    //                < td > @T.Email </ td >
    //                < td > @T.WeeklyHours.TotalHours </ td >
    //                < td > @T.HasCar </ td >
    //                < td > @T.Address.RoadName @T.Address.RoadNumber, @T.Address.City </ td >
    //                < td >
    //                    @foreach(var admin in T.AdminIDs)
    //                    {
    //    foreach (var Admin in Model.Admins)
    //    {
    //        if (admin == Admin.AdministratorID)
    //        {
    //                                < p > @Admin.FirstName @Admin.LastName </ p >
    //                            }

    //    }
    //}
    //                </ td >
    //                < td >
    //                    < form method = "post" asp - page - handler = "EditTeacherProfile" class= "btn padding-none" >
    //                        < input type = "hidden" asp -for= "TeacherID" value = "@T.TeacherID" />
    //                        < input class= "no-border" type = "submit" value = "Edit" />
    //                    </ form >
    //                </ td >
    //                < td >
    //                    < form method = "post" asp - page - handler = "DeleteTeacher" class= "btn padding-none" >
    //                        < input class= "no-border" type = "submit" value = "Remove" />
    //                    </ form >
    //                </ td >
    //            </ tr >
    //        }
    //    </ tbody >
    //</ table >