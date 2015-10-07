var ViewModel = function () {
        var self = this;
        self.Jobs = ko.observableArray();
        self.search = ko.observable('');
        self.job = ko.observable();
        
        self.GJobs = function (search) {
            GetJobs(self.search);
        }
        self.GetMenu = function (JobID) {
            GetMenu(JobID);
        }

        

        function GetMenuPhone() {
            $.ajax({
                type: "GET",
                url: 'PhoneMechanic/MenuMobile',
                success: function (data) {
                    $('#MechMobile').html(data);
                    $('#MechMobile').fadeIn("slow");
                },
                error: function (data) {
                    $('#MechMobile').html('<h3>Error in dsssretrieval</h3>');
                }
            });
        }

        function GetJobs(search) {
            $.ajax({
                type: "GET",
                url: 'api/mechanicphone/',
                data: { reg: search },
                dataType: 'json',
                contentType: 'application/json',
                success: function (data) {
                    self.Jobs(data);
                   
                },
                error: function (data) {
                    $('#MechMobile').html('<h3>Error in retrieval</h3>');
                }
            });
        }

        
        function GetMenu(JobID) {
            $.ajax({
                type: "GET",
                url: 'api/mechanicphone/Details',
                data: { id: JobID },
                dataType: 'json',
                contentType: 'application/json',
                success: function (data) {
                self.job(data);
                GetMenuPhone();
                    
                },
                error: function (data) {
                    $('#MechMobile').html('<h3>Error in retrieval</h3>');
                }
            });
        }}

      
ko.applyBindings(new ViewModel());
