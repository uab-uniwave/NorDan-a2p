namespace a2p.WinForm.CustomControls
{
    public class SideBarButton : Button
    {
        private bool selectedOne = true; // Backing field

        public bool SelectedOne
        {
            get => selectedOne;
            set
            {
                if (selectedOne!=value)
                {
                    selectedOne=value;
                    OnSelectedOneChanged(EventArgs.Empty); // Raise the event
                }
            }
        }

        // Event for property change
        public event EventHandler SelectedOneChanged;

        protected virtual void OnSelectedOneChanged(EventArgs e)
        {
            SelectedOneChanged.Invoke(this, e);
        }

        public SideBarButton()
        {
            SelectedOneChanged+=(sender, e) => { }; // Initialize the event with an empty delegate
            Margin=new Padding(0);
            Dock=DockStyle.Top;
            FlatStyle=FlatStyle.Flat;
            FlatAppearance.BorderSize=1;


            TextAlign=ContentAlignment.MiddleLeft;
            ImageAlign=ContentAlignment.MiddleLeft;
            TextImageRelation=TextImageRelation.ImageBeforeText;
            TextAlign=ContentAlignment.MiddleLeft;
            FlatAppearance.BorderSize=1;

            BackColor=Color.Transparent;
            ForeColor=UniwaveColors.uwOrangeDeep;
            Font=new Font("Segoe UI", 8.5F, FontStyle.Bold);
            FlatAppearance.BorderColor=UniwaveColors.a2pGreyDark;
            FlatAppearance.BorderSize=0;



            if (Enabled==true)
            {
                // Add hover effects
                MouseEnter+=(sender, e) =>
                {
                    BackColor=UniwaveColors.uwOrangeDeep;
                    ForeColor=UniwaveColors.uwGreyLight;
                    Font=new Font("Segoe UI", 9F, FontStyle.Bold);
                    FlatAppearance.BorderColor=UniwaveColors.uwGreyLight;
                };

                MouseLeave+=(sender, e) =>
                {
                    BackColor=Color.Transparent;
                    ForeColor=UniwaveColors.uwOrangeDeep;
                    Font=new Font("Segoe UI", 8.5F, FontStyle.Bold);
                    FlatAppearance.BorderColor=UniwaveColors.a2pGreyDark;
                    FlatAppearance.BorderSize=0;
                };

                // Add focus effects
                SelectedOneChanged+=(sender, e) =>
                {

                    if (sender!=null)
                    {
                        if (sender.ToString()!=string.Empty)
                        {
                            if (SelectedOne)
                            {
                                BackColor=UniwaveColors.uwOrangeDeep;
                                ForeColor=UniwaveColors.uwGreyLight;
                                Font=new Font("Segoe UI", 9F, FontStyle.Bold);
                                FlatAppearance.BorderColor=UniwaveColors.uwGreyLight;
                                FlatAppearance.BorderSize=1;
                            }
                            else
                            {

                                BackColor=Color.Transparent;
                                ForeColor=UniwaveColors.uwOrangeDeep;
                                Font=new Font("Segoe UI", 8.5F, FontStyle.Bold);
                                FlatAppearance.BorderColor=UniwaveColors.a2pGreyDark;
                                FlatAppearance.BorderSize=0;

                            }
                        }
                    }
                };

                // Add click effects
                MouseDown+=(sender, e) =>
                {
                    BackColor=UniwaveColors.uwGreyLight;
                    ForeColor=UniwaveColors.uwOrangeDeep;
                    Font=new Font("Segoe UI", 8.5F, FontStyle.Bold);
                    FlatAppearance.BorderColor=UniwaveColors.uwOrangeDeep;
                    FlatAppearance.BorderSize=1;
                };

                MouseUp+=(sender, e) =>
                {
                    BackColor=UniwaveColors.uwOrangeDeep;
                    ForeColor=UniwaveColors.uwGreyLight;
                    Font=new Font("Segoe UI", 8F, FontStyle.Bold);
                    FlatAppearance.BorderColor=UniwaveColors.uwGreyLight;
                    FlatAppearance.BorderSize=1;
                };


            }

            // Define how button looks like when it is disabled
            EnabledChanged+=(sender, e) =>
            {
                if (Enabled&&SelectedOne)
                {
                    BackColor=UniwaveColors.uwOrangeDeep;
                    ForeColor=UniwaveColors.uwGreyLight;
                    Font=new Font("Segoe UI", 9F, FontStyle.Bold);
                    FlatAppearance.BorderColor=UniwaveColors.uwGreyLight;
                    FlatAppearance.BorderSize=1;

                }
                else if (Enabled&&!SelectedOne)
                {
                    BackColor=Color.Transparent;
                    ForeColor=UniwaveColors.uwOrangeDeep;
                    Font=new Font("Segoe UI", 9F, FontStyle.Bold);
                    FlatAppearance.BorderColor=UniwaveColors.a2pGreyDark;
                    FlatAppearance.BorderSize=0;

                }
                else if (!Enabled&&!SelectedOne)
                {
                    BackColor=Color.Gray;
                    ForeColor=Color.DarkGray;
                    Font=new Font("Segoe UI", 8.5F, FontStyle.Bold);
                    FlatAppearance.BorderColor=UniwaveColors.a2pGreyDark;
                    FlatAppearance.BorderSize=0;
                }
                else if (!Enabled&&SelectedOne)
                {
                    BackColor=Color.Gray;
                    ForeColor=Color.DarkGray;
                    Font=new Font("Segoe UI", 8.5F, FontStyle.Bold);
                    ForeColor=Color.DarkGray;
                    FlatAppearance.BorderSize=1;


                }

            };
        }
    }
}