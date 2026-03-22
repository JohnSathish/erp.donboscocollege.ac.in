import { Routes } from '@angular/router';
import { AuthComponent } from './auth/auth.component';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'index',
    pathMatch: 'full'
  },

    {
        path: '',
        loadComponent: () => import('./auth/auth.component').then((m) => m.AuthComponent),
        children: [
           {path:'login',loadComponent: ()=> import('./auth/login/login/login.component').then((m)=> m.LoginComponent)},
           {path:'login-2',loadComponent: ()=> import('./auth/login/login2/login2.component').then((m)=> m.Login2Component)},
           {path:'login-3',loadComponent: ()=> import('./auth/login/login3/login3.component').then((m)=> m.Login3Component)},

            { path: 'register', loadComponent: () => import('./auth/register/register/register.component').then(m => m.RegisterComponent) },
            { path: 'register-2', loadComponent: () => import('./auth/register/register2/register2.component').then(m => m.Register2Component) },
            { path: 'register-3', loadComponent: () => import('./auth/register/register3/register3.component').then(m => m.Register3Component) },

            { path: 'forgot-password', loadComponent: () => import('./auth/forget-password/forget-password/forget-password.component').then(m => m.ForgetPasswordComponent) },
            { path: 'forgot-password-2', loadComponent: () => import('./auth/forget-password/forget-password2/forget-password2.component').then(m => m.ForgetPassword2Component) },
            { path: 'forgot-password-3', loadComponent: () => import('./auth/forget-password/forget-password3/forget-password3.component').then(m => m.ForgetPassword3Component) },

            { path: 'reset-password', loadComponent: () => import('./auth/reset-password/reset-password/reset-password.component').then(m => m.ResetPasswordComponent) },
            { path: 'reset-password-2', loadComponent: () => import('./auth/reset-password/reset-password-2/reset-password-2.component').then(m => m.ResetPassword2Component) },
            { path: 'reset-password-3', loadComponent: () => import('./auth/reset-password/reset-password-3/reset-password-3.component').then(m => m.ResetPassword3Component) },

            { path: 'reset-password-success', loadComponent: () => import('./auth/reset-password-success/reset-password-success/reset-password-success.component').then(m => m.ResetPasswordSuccessComponent) },
            { path: 'reset-password-success-2', loadComponent: () => import('./auth/reset-password-success/reset-password-success-2/reset-password-success-2.component').then(m => m.ResetPasswordSuccess2Component) },
            { path: 'reset-password-success-3', loadComponent: () => import('./auth/reset-password-success/reset-password-success-3/reset-password-success-3.component').then(m => m.ResetPasswordSuccess3Component) },

            { path: 'two-step-verification', loadComponent: () => import('./auth/two-step-verification/two-step-verification/two-step-verification.component').then(m => m.TwoStepVerificationComponent) },
            { path: 'two-step-verification-2', loadComponent: () => import('./auth/two-step-verification/two-step-verification-2/two-step-verification-2.component').then(m => m.TwoStepVerification2Component) },
            { path: 'two-step-verification-3', loadComponent: () => import('./auth/two-step-verification/two-step-verification-3/two-step-verification-3.component').then(m => m.TwoStepVerification3Component) },

            { path: 'email-verification', loadComponent: () => import('./auth/email-verification/email-verification/email-verification.component').then(m => m.EmailVerificationComponent) },
            { path: 'email-verification-2', loadComponent: () => import('./auth/email-verification/email-verification-2/email-verification-2.component').then(m => m.EmailVerification2Component) },
            { path: 'email-verification-3', loadComponent: () => import('./auth/email-verification/email-verification-3/email-verification-3.component').then(m => m.EmailVerification3Component) },

            { path: 'lock-screen', loadComponent: () => import('./auth/lock-screen/lock-screen.component').then(m => m.LockScreenComponent) },
        ]
    },
     //Error
    { path: 'error', loadComponent: () => import('./error/error.component').then((m) => m.ErrorComponent,),
        children: [
          { path: '', loadComponent: () => import('./error/error-404/error-404.component').then((m) => m.Error404Component), },
          { path: 'error-404', loadComponent: () => import('./error/error-404/error-404.component').then((m) => m.Error404Component), },
          { path: 'error-500', loadComponent: () => import('./error/error-500/error-500.component').then((m) => m.Error500Component),},
        ],
      },
     {
        path: '',
        loadComponent: () => import('./features/features.component').then((m) => m.FeaturesComponent),
        children:[
            { path: 'index', loadComponent: () => import('./features/dashboards/admin-dashboard/admin-dashboard.component').then(m => m.AdminDashboardComponent)},
            { path: 'parent-dashboard',loadComponent: () => import('./features/dashboards/parent-dashboard/parent-dashboard.component').then(m => m.ParentDashboardComponent)},
            { path: 'teacher-dashboard', loadComponent: () => import('./features/dashboards/teacher-dashboard/teacher-dashboard.component').then(m => m.TeacherDashboardComponent)},
            { path: 'student-dashboard', loadComponent: () => import('./features/dashboards/student-dashboard/student-dashboard.component').then(m => m.StudentDashboardComponent)},
            {path: 'layout-rtl', loadComponent: () => import('./features/dashboards/admin-dashboard/admin-dashboard.component').then(m => m.AdminDashboardComponent)},
            { path: 'application',
              loadComponent: () => import('./features/features.component').then((m) => m.FeaturesComponent),
              children:[
                  { path: 'chat',loadComponent: () => import('./features/application/chat/chat.component').then(m => m.ChatComponent) },
                  { path: 'calendar', loadComponent: () => import('./features/application/calendar/calendar.component').then(m => m.CalendarComponent)},
                  { path: 'email', loadComponent: () => import('./features/application/email/email.component').then(m => m.EmailComponent)},
                  { path: 'call-history', loadComponent: () => import('./features/application/call/call-history/call-history.component').then(m => m.CallHistoryComponent) },
                  { path: 'file-manager', loadComponent: () => import('./features/application/file-manager/file-manager.component').then(m => m.FileManagerComponent) },
                  { path: 'audio-call', loadComponent: () => import('./features/application/call/audio-call/audio-call.component').then(m => m.AudioCallComponent) },
                  { path: 'video-call', loadComponent: () => import('./features/application/call/video-call/video-call.component').then(m => m.VideoCallComponent) },
                  { path: 'todo', loadComponent: () => import('./features/application/todo/todo.component').then(m => m.TodoComponent) },
                  { path: 'notes', loadComponent: () => import('./features/application/notes/notes.component').then(m => m.NotesComponent) },
              ]
            },
            { path: 'base-ui', loadComponent: () => import('./features/ui-interface/base-ui/base-ui.component').then(m => m.BaseUiComponent),
              children: [
                    { path: 'ui-spinner', loadComponent: () => import('./features/ui-interface/base-ui/ui-spinner/ui-spinner.component').then(m => m.UiSpinnerComponent) },
                    { path: 'ui-rangeslider', loadComponent: () => import('./features/ui-interface/base-ui/ui-rangeslider/ui-rangeslider.component').then(m => m.UiRangesliderComponent) },
                    { path: 'ui-progress', loadComponent: () => import('./features/ui-interface/base-ui/ui-progress/ui-progress.component').then(m => m.UiProgressComponent) },
                    { path: 'ui-video', loadComponent: () => import('./features/ui-interface/base-ui/ui-video/ui-video.component').then(m => m.UiVideoComponent) },
                    { path: 'ui-typography', loadComponent: () => import('./features/ui-interface/base-ui/ui-typography/ui-typography.component').then(m => m.UiTypographyComponent) },
                    { path: 'ui-carousel', loadComponent: () => import('./features/ui-interface/base-ui/ui-carousel/ui-carousel.component').then(m => m.UiCarouselComponent) },
                    { path: 'ui-cards', loadComponent: () => import('./features/ui-interface/base-ui/ui-cards/ui-cards.component').then(m => m.UiCardsComponent) },
                    { path: 'ui-buttons-group', loadComponent: () => import('./features/ui-interface/base-ui/ui-buttons-group/ui-buttons-group.component').then(m => m.UiButtonsGroupComponent) },
                    { path: 'ui-buttons', loadComponent: () => import('./features/ui-interface/base-ui/ui-buttons/ui-buttons.component').then(m => m.UiButtonsComponent) },
                    { path: 'ui-breadcrumb', loadComponent: () => import('./features/ui-interface/base-ui/ui-breadcrumb/ui-breadcrumb.component').then(m => m.UiBreadcrumbComponent) },
                    { path: 'ui-borders', loadComponent: () => import('./features/ui-interface/base-ui/ui-borders/ui-borders.component').then(m => m.UiBordersComponent) },
                    { path: 'ui-badges', loadComponent: () => import('./features/ui-interface/base-ui/ui-badges/ui-badges.component').then(m => m.UiBadgesComponent) },
                    { path: 'ui-accordion', loadComponent: () => import('./features/ui-interface/base-ui/ui-accordion/ui-accordion.component').then(m => m.UiAccordionComponent) },
                    { path: 'ui-alerts', loadComponent: () => import('./features/ui-interface/base-ui/ui-alerts/ui-alerts.component').then(m => m.UiAlertsComponent) },
                    { path: 'ui-avatar', loadComponent: () => import('./features/ui-interface/base-ui/ui-avatar/ui-avatar.component').then(m => m.UiAvatarComponent) },
                    { path: 'ui-popovers', loadComponent: () => import('./features/ui-interface/base-ui/ui-popovers/ui-popovers.component').then(m => m.UiPopoversComponent) },
                    { path: 'ui-placeholders', loadComponent: () => import('./features/ui-interface/base-ui/ui-placeholders/ui-placeholders.component').then(m => m.UiPlaceholdersComponent) },
                    { path: 'ui-pagination', loadComponent: () => import('./features/ui-interface/base-ui/ui-pagination/ui-pagination.component').then(m => m.UiPaginationComponent) },
                    { path: 'ui-offcanvas', loadComponent: () => import('./features/ui-interface/base-ui/ui-offcanvas/ui-offcanvas.component').then(m => m.UiOffcanvasComponent) },
                    { path: 'ui-nav-tabs', loadComponent: () => import('./features/ui-interface/base-ui/ui-nav-tabs/ui-nav-tabs.component').then(m => m.UiNavTabsComponent) },
                    { path: 'ui-modals', loadComponent: () => import('./features/ui-interface/base-ui/ui-modals/ui-modals.component').then(m => m.UiModalsComponent) },
                    { path: 'ui-media', loadComponent: () => import('./features/ui-interface/base-ui/ui-media/ui-media.component').then(m => m.UiMediaComponent) },
                    { path: 'ui-lightbox', loadComponent: () => import('./features/ui-interface/base-ui/ui-lightbox/ui-lightbox.component').then(m => m.UiLightboxComponent) },
                    { path: 'ui-images', loadComponent: () => import('./features/ui-interface/base-ui/ui-images/ui-images.component').then(m => m.UiImagesComponent) },
                    { path: 'ui-grid', loadComponent: () => import('./features/ui-interface/base-ui/ui-grid/ui-grid.component').then(m => m.UiGridComponent) },
                    { path: 'ui-tooltips', loadComponent: () => import('./features/ui-interface/base-ui/ui-tooltips/ui-tooltips.component').then(m => m.UiTooltipsComponent) },
                    { path: 'ui-toasts', loadComponent: () => import('./features/ui-interface/base-ui/ui-toasts/ui-toasts.component').then(m => m.UiToastsComponent) },
                    { path: 'ui-dropdowns', loadComponent: () => import('./features/ui-interface/base-ui/ui-dropdowns/ui-dropdowns.component').then(m => m.UiDropdownsComponent) },
                    { path: 'ui-colors', loadComponent: () => import('./features/ui-interface/base-ui/ui-colors/ui-colors.component').then(m => m.UiColorsComponent) },
                  ]
            },
            { path: 'advanced-ui', loadComponent: () => import('./features/ui-interface/advanced-ui/advanced-ui.component').then(m => m.AdvancedUiComponent),
              children: [
                  { path: 'ui-timeline', loadComponent: () => import('./features/ui-interface/advanced-ui/ui-timeline/ui-timeline.component').then(m => m.UiTimelineComponent) },
                  { path: 'ui-text-editor', loadComponent: () => import('./features/ui-interface/advanced-ui/ui-text-editor/ui-text-editor.component').then(m => m.UiTextEditorComponent) },
                  { path: 'ui-scrollbar', loadComponent: () => import('./features/ui-interface/advanced-ui/ui-scrollbar/ui-scrollbar.component').then(m => m.UiScrollbarComponent) },
                  { path: 'ui-ribbon', loadComponent: () => import('./features/ui-interface/advanced-ui/ui-ribbon/ui-ribbon.component').then(m => m.UiRibbonComponent) },
                  { path: 'ui-rating', loadComponent: () => import('./features/ui-interface/advanced-ui/ui-rating/ui-rating.component').then(m => m.UiRatingComponent) },
                  { path: 'ui-drag-drop', loadComponent: () => import('./features/ui-interface/advanced-ui/ui-drag-drop/ui-drag-drop.component').then(m => m.UiDragDropComponent) },
                  { path: 'ui-counter', loadComponent: () => import('./features/ui-interface/advanced-ui/ui-counter/ui-counter.component').then(m => m.UiCounterComponent) },
                  { path: 'ui-clipboard', loadComponent: () => import('./features/ui-interface/advanced-ui/ui-clipboard/ui-clipboard.component').then(m => m.UiClipboardComponent) },
                ]               
            },
            { path: 'charts', loadComponent: () => import('./features/ui-interface/charts/charts.component').then(m => m.ChartsComponent),
              children: [
                  { path: 'prime-ng', loadComponent: () => import('./features/ui-interface/charts/prime-ng/prime-ng.component').then(m => m.PrimeNgComponent) },
                  { path: 'chart-apex', loadComponent: () => import('./features/ui-interface/charts/chart-apex/chart-apex.component').then(m => m.ChartApexComponent) },
                ]

            },
            { path: 'forms', loadComponent: () => import('./features/ui-interface/forms/forms.component').then(m => m.FormsComponent),
              children: [
                    { path: 'form-basic-inputs', loadComponent: () => import('./features/ui-interface/forms/form-elements/form-basic-inputs/form-basic-inputs.component').then(m => m.FormBasicInputsComponent) },
                    { path: 'form-checkbox-radios', loadComponent: () => import('./features/ui-interface/forms/form-elements/form-checkbox-radios/form-checkbox-radios.component').then(m => m.FormCheckboxRadiosComponent) },
                    { path: 'form-fileupload', loadComponent: () => import('./features/ui-interface/forms/form-elements/form-fileupload/form-fileupload.component').then(m => m.FormFileuploadComponent) },
                    { path: 'form-grid-gutters', loadComponent: () => import('./features/ui-interface/forms/form-elements/form-grid-gutters/form-grid-gutters.component').then(m => m.FormGridGuttersComponent) },
                    { path: 'form-input-groups', loadComponent: () => import('./features/ui-interface/forms/form-elements/form-input-groups/form-input-groups.component').then(m => m.FormInputGroupsComponent) },
                    { path: 'form-select', loadComponent: () => import('./features/ui-interface/forms/form-elements/form-select/form-select.component').then(m => m.FormSelectComponent) },
                    { path: 'form-validation', loadComponent: () => import('./features/ui-interface/forms/form-validation/form-validation.component').then(m => m.FormValidationComponent) },
                    { path: 'form-floating-labels', loadComponent: () => import('./features/ui-interface/forms/layouts/form-floating-labels/form-floating-labels.component').then(m => m.FormFloatingLabelsComponent) },
                    { path: 'form-horizontal', loadComponent: () => import('./features/ui-interface/forms/layouts/form-horizontal/form-horizontal.component').then(m => m.FormHorizontalComponent) },
                    { path: 'form-vertical', loadComponent: () => import('./features/ui-interface/forms/layouts/form-vertical/form-vertical.component').then(m => m.FormVerticalComponent) },
                    { path: 'form-mask', loadComponent: () => import('./features/ui-interface/forms/form-elements/form-mask/form-mask.component').then(m => m.FormMaskComponent) },
                  ]

            },
            { path: 'table', loadComponent: () => import('./features/ui-interface/table/table.component').then(m => m.TableComponent),
              children:[
                      { path: 'tables-basic', loadComponent: () => import('./features/ui-interface/table//tables-basic/tables-basic.component').then(m => m.TablesBasicComponent) },
                      { path: 'data-basic', loadComponent: () => import('./features/ui-interface/table//data-tables/data-tables.component').then(m => m.DataTablesComponent) },

              ]
            },
            { path: 'icons', loadComponent: () => import('./features/ui-interface/icons/icons.component').then(m => m.IconsComponent),
              children:[
                { path: 'icon-fontawesome', loadComponent: () => import('./features/ui-interface/icons/icon-fontawesome/icon-fontawesome.component').then(m => m.IconFontawesomeComponent) },
                { path: 'icon-feather', loadComponent: () => import('./features/ui-interface/icons/icon-feather/icon-feather.component').then(m => m.IconFeatherComponent) },
                { path: 'icon-ionic', loadComponent: () => import('./features/ui-interface/icons/icon-ionic/icon-ionic.component').then(m => m.IconIonicComponent) },
                { path: 'icon-material', loadComponent: () => import('./features/ui-interface/icons/icon-material/icon-material.component').then(m => m.IconMaterialComponent) },
                { path: 'icon-pe7', loadComponent: () => import('./features/ui-interface/icons/icon-pe7/icon-pe7.component').then(m => m.IconPe7Component) },
                { path: 'icon-simpleline', loadComponent: () => import('./features/ui-interface/icons/icon-simpleline/icon-simpleline.component').then(m => m.IconSimplelineComponent) },
                { path: 'icon-themify', loadComponent: () => import('./features/ui-interface/icons/icon-themify/icon-themify.component').then(m => m.IconThemifyComponent) },
                { path: 'icon-weather', loadComponent: () => import('./features/ui-interface/icons/icon-weather/icon-weather.component').then(m => m.IconWeatherComponent) },
                { path: 'icon-typicon', loadComponent: () => import('./features/ui-interface/icons/icon-typicon/icon-typicon.component').then(m => m.IconTypiconComponent) },
                { path: 'icon-flag', loadComponent: () => import('./features/ui-interface/icons/icon-flag/icon-flag.component').then(m => m.IconFlagComponent) },
              ]
            },
            { path: 'teachers',loadComponent:() => import('./features/peoples/teachers/teachers.component').then(m => m.TeachersComponent),
              children: [
                  { path: 'add-teacher', loadComponent: () => import('./features/peoples/teachers/add-teacher/add-teacher.component').then(m => m.AddTeacherComponent) },
                  { path: 'edit-teacher', loadComponent: () => import('./features/peoples/teachers/edit-teacher/edit-teacher.component').then(m => m.EditTeacherComponent) },
                  { path: 'teacher-details', loadComponent: () => import('./features/peoples/teachers/teacher-details/teacher-details.component').then(m => m.TeacherDetailsComponent) },
                  { path: 'teacher-grid', loadComponent: () => import('./features/peoples/teachers/teacher-grid/teacher-grid.component').then(m => m.TeacherGridComponent) },
                  { path: 'teacher-leaves', loadComponent: () => import('./features/peoples/teachers/teacher-leaves/teacher-leaves.component').then(m => m.TeacherLeavesComponent) },
                  { path: 'teacher-library', loadComponent: () => import('./features/peoples/teachers/teacher-library/teacher-library.component').then(m => m.TeacherLibraryComponent) },
                  { path: 'teacher-salary', loadComponent: () => import('./features/peoples/teachers/teacher-salary/teacher-salary.component').then(m => m.TeacherSalaryComponent) },
                  { path: 'teachers-list', loadComponent: () => import('./features/peoples/teachers/teachers-list/teachers-list.component').then(m => m.TeachersListComponent) },
                  { path: 'teachers-routine', loadComponent: () => import('./features/peoples/teachers/teachers-routine/teachers-routine.component').then(m => m.TeachersRoutineComponent) },
                ]
            },
            //Peoples
            { path: 'students',loadComponent:() => import('./features/peoples/students/students.component').then(m => m.StudentsComponent),
              children: [
                  { path: 'student-result', loadComponent: () => import('./features/peoples/students/student-result/student-result.component').then(m => m.StudentResultComponent) },
                  { path: 'edit-student', loadComponent: () => import('./features/peoples/students/edit-student/edit-student.component').then(m => m.EditStudentComponent) },
                  { path: 'add-student', loadComponent: () => import('./features/peoples/students/add-student/add-student.component').then(m => m.AddStudentComponent) },
                  { path: 'student-time-table', loadComponent: () => import('./features/peoples/students/student-time-table/student-time-table.component').then(m => m.StudentTimeTableComponent) },
                  { path: 'students-list', loadComponent: () => import('./features/peoples/students/students-list/students-list.component').then(m => m.StudentsListComponent) },
                  { path: 'student-fees', loadComponent: () => import('./features/peoples/students/student-fees/student-fees.component').then(m => m.StudentFeesComponent) },
                  { path: 'student-library', loadComponent: () => import('./features/peoples/students/student-library/student-library.component').then(m => m.StudentLibraryComponent) },
                  { path: 'student-leaves', loadComponent: () => import('./features/peoples/students/student-leaves/student-leaves.component').then(m => m.StudentLeavesComponent) },
                  { path: 'student-promotion', loadComponent: () => import('./features/peoples/students/student-promotion/student-promotion.component').then(m => m.StudentPromotionComponent) },
                  { path: 'student-grid', loadComponent: () => import('./features/peoples/students/student-grid/student-grid.component').then(m => m.StudentGridComponent) },
                  { path: 'student-details', loadComponent: () => import('./features/peoples/students/student-details/student-details.component').then(m => m.StudentDetailsComponent) },
                ]
            },
            { path: 'parents',loadComponent:() => import('./features/peoples/parents/parents.component').then(m => m.ParentsComponent),
              children: [
                  { path: 'parent-grid', loadComponent: () => import('./features/peoples/parents/parent-grid/parent-grid.component').then(m => m.ParentGridComponent) },
                  { path: 'parents-list', loadComponent: () => import('./features/peoples/parents/parents-list/parents-list.component').then(m => m.ParentsListComponent) },
                ]
            },
            { path: 'guardians',loadComponent:() => import('./features/peoples/guardians/guardians.component').then(m => m.GuardiansComponent),
              children: [
                  { path: 'guardian-grid', loadComponent: () => import('./features/peoples/guardians/guardian-grid/guardian-grid.component').then(m => m.GuardianGridComponent) },
                  { path: 'guardian-list', loadComponent: () => import('./features/peoples/guardians/guardian-list/guardian-list.component').then(m => m.GuardianListComponent) },
                ]
            },
            //Academics
             { path: 'classes', loadComponent: () => import('./features/academic/classes/classes.component').then(m => m.ClassesComponent),
              children:[
                      { path: 'class-list', loadComponent: () => import('./features/academic/classes/class-list/class-list.component').then(m => m.ClassListComponent) },
                      { path: 'schedule-classes', loadComponent: () => import('./features/academic/classes/schedule-classes/schedule-classes.component').then(m => m.ScheduleClassesComponent)},
              ]
              },
              { path: 'academic-reasons', loadComponent: () => import('./features/academic/academic-reasons/academic-reasons.component').then(m => m.AcademicReasonsComponent) },
              { path: 'class-room', loadComponent: () => import('./features/academic/classroom/classroom.component').then(m => m.ClassroomComponent) },
              { path: 'class-section', loadComponent: () => import('./features/academic/class-section/class-section.component').then(m => m.ClassSectionComponent) },
              { path: 'class-routine', loadComponent: () => import('./features/academic/class-routine/class-routine.component').then(m => m.ClassRoutineComponent) },
              { path: 'class-time-table', loadComponent: () => import('./features/academic/class-time-table/class-time-table.component').then(m => m.ClassTimeTableComponent) },
              { path: 'class-syllabus', loadComponent: () => import('./features/academic/class-syllabus/class-syllabus.component').then(m => m.ClassSyllabusComponent) },
              { path: 'class-subject', loadComponent: () => import('./features/academic/class-subject/class-subject.component').then(m => m.ClassSubjectComponent) },
              { path: 'class-home-work', loadComponent: () => import('./features/academic/class-home-work/class-home-work.component').then(m => m.ClassHomeWorkComponent) },
              { path: 'examinations', loadComponent: () => import('./features/academic/examinations/examinations.component').then(m => m.ExaminationsComponent),
                children: [
              { path: 'grade', loadComponent: () => import('./features/academic/examinations/grade/grade.component').then(m => m.GradeComponent) },
              { path: 'exam-list', loadComponent: () => import('./features/academic/examinations/exam-list/exam-list.component').then(m => m.ExamListComponent) },
              { path: 'exam-schedule', loadComponent: () => import('./features/academic/examinations/exam-schedule/exam-schedule.component').then(m => m.ExamScheduleComponent) },
              { path: 'exam-attendance', loadComponent: () => import('./features/academic/examinations/exam-attendance/exam-attendance.component').then(m => m.ExamAttendanceComponent) },
              { path: 'exam-results', loadComponent: () => import('./features/academic/examinations/exam-results/exam-results.component').then(m => m.ExamResultsComponent) }
            ]
              },
          //Management
          { path: 'transport', loadComponent: () => import('./features/management/transport/transport.component').then(m => m.TransportComponent),
            children: [
                { path: 'transport-vehicle', loadComponent: () => import('./features/management/transport/transport-vehicle/transport-vehicle.component').then(m => m.TransportVehicleComponent) },
                { path: 'transport-vehicle-drivers', loadComponent: () => import('./features/management/transport/transport-vehicle-drivers/transport-vehicle-drivers.component').then(m => m.TransportVehicleDriversComponent) },
                { path: 'transport-assign-vehicle', loadComponent: () => import('./features/management/transport/transport-assign-vehicle/transport-assign-vehicle.component').then(m => m.TransportAssignVehicleComponent) },
                { path: 'transport-pickup-points', loadComponent: () => import('./features/management/transport/transport-pickup-points/transport-pickup-points.component').then(m => m.TransportPickupPointsComponent) },
                { path: 'trasnsport-routes', loadComponent: () => import('./features/management/transport/transport-routes/transport-routes.component').then(m => m.TransportRoutesComponent) }
              ]
           },
          { path: 'fees-collection', loadComponent: () => import('./features/management/fees-collection/fees-collection.component').then(m => m.FeesCollectionComponent),
            children: [
                { path: 'fees-group', loadComponent: () => import('./features/management/fees-collection/fees-group/fees-group.component').then(m => m.FeesGroupComponent) },
                { path: 'fees-master', loadComponent: () => import('./features/management/fees-collection/fees-master/fees-master.component').then(m => m.FeesMasterComponent) },
                { path: 'fees-type', loadComponent: () => import('./features/management/fees-collection/fees-type/fees-type.component').then(m => m.FeesTypeComponent) },
                { path: 'collect-fees', loadComponent: () => import('./features/management/fees-collection/collect-fees/collect-fees.component').then(m => m.CollectFeesComponent) },
                { path: 'fees-assign', loadComponent: () => import('./features/management/fees-collection/fees-assign/fees-assign.component').then(m => m.FeesAssignComponent) }
              ]
           },
          { path: 'hostel', loadComponent: () => import('./features/management/hostel/hostel.component').then(m => m.HostelComponent),
            children: [
                { path: 'hostel-rooms', loadComponent: () => import('./features/management/hostel/hostel-rooms/hostel-rooms.component').then(m => m.HostelRoomsComponent) },
                { path: 'hostel-room-type', loadComponent: () => import('./features/management/hostel/hostel-room-type/hostel-room-type.component').then(m => m.HostelRoomTypeComponent) },
                { path: 'hostel-list', loadComponent: () => import('./features/management/hostel/hostel-list/hostel-list.component').then(m => m.HostelListComponent) }
              ]
           },
          { path: 'sports', loadComponent: () => import('./features/management/sports/sports.component').then(m => m.SportsComponent) },
          { path: 'players', loadComponent: () => import('./features/management/players/players.component').then(m => m.PlayersComponent) },
          { path: 'library', loadComponent: () => import('./features/management/library/library.component').then(m => m.LibraryComponent),
            children: [
            { path: 'return-book', loadComponent: () => import('./features/management/library/return-book/return-book.component').then(m => m.ReturnBookComponent) },
            { path: 'issue-book', loadComponent: () => import('./features/management/library/issue-book/issue-book.component').then(m => m.IssueBookComponent) },
            { path: 'books', loadComponent: () => import('./features/management/library/books/books.component').then(m => m.BooksComponent) },
            { path: 'library-members', loadComponent: () => import('./features/management/library/library-members/library-members.component').then(m => m.LibraryMembersComponent) }
          ]
           },
           //HRM
            { path: 'leaves', loadComponent: () => import('./features/hrm/leaves/leaves.component').then(m => m.LeavesComponent),
              children: [
              { path: 'approve-request', loadComponent: () => import('./features/hrm/leaves/approve-request/approve-request.component').then(m => m.ApproveRequestComponent) },
              { path: 'list-leaves', loadComponent: () => import('./features/hrm/leaves/list-leaves/list-leaves.component').then(m => m.ListLeavesComponent) }
            ]
             },
            { path: 'departments', loadComponent: () => import('./features/hrm/departments/departments.component').then(m => m.DepartmentsComponent) },
            { path: 'designation', loadComponent: () => import('./features/hrm/designation/designation.component').then(m => m.DesignationComponent) },
            { path: 'holidays', loadComponent: () => import('./features/hrm/holidays/holidays.component').then(m => m.HolidaysComponent) },
            { path: 'payroll', loadComponent: () => import('./features/hrm/payroll/payroll.component').then(m => m.PayrollComponent) },
            { path: 'staff', loadComponent: () => import('./features/hrm/staff/staff.component').then(m => m.StaffComponent),
              children: [
                { path: 'staff-details', loadComponent: () => import('./features/hrm/staff/staff-details/staff-details.component').then(m => m.StaffDetailsComponent) },
                { path: 'staff-leaves', loadComponent: () => import('./features/hrm/staff/staff-leaves/staff-leaves.component').then(m => m.StaffLeavesComponent) },
                { path: 'staff-payroll', loadComponent: () => import('./features/hrm/staff/staff-payroll/staff-payroll.component').then(m => m.StaffPayrollComponent) },
                { path: 'add-staff', loadComponent: () => import('./features/hrm/staff/add-staff/add-staff.component').then(m => m.AddStaffComponent) },
                { path: 'edit-staff', loadComponent: () => import('./features/hrm/staff/edit-staff/edit-staff.component').then(m => m.EditStaffComponent) },
                { path: 'staff-list', loadComponent: () => import('./features/hrm/staff/staff-list/staff-list.component').then(m => m.StaffListComponent) }
              ]
             },
            { path: 'attendance', loadComponent: () => import('./features/hrm/attendance/attendance.component').then(m => m.AttendanceComponent),
              children: [
                  { path: 'student-attendance', loadComponent: () => import('./features/hrm/attendance/student-attendance/student-attendance.component').then(m => m.StudentAttendanceComponent) },
                  { path: 'staff-attendance', loadComponent: () => import('./features/hrm/attendance/staff-attendance/staff-attendance.component').then(m => m.StaffAttendanceComponent) },
                  { path: 'teacher-attendance', loadComponent: () => import('./features/hrm/attendance/teacher-attendance/teacher-attendance.component').then(m => m.TeacherAttendanceComponent) },
                  { path: 'staffs-attendance', loadComponent: () => import('./features/hrm/attendance/staffs-attendance/staffs-attendance.component').then(m => m.StaffsAttendanceComponent) }
                ]
             },

           //Finance & Accounts
            { path: 'accounts', loadComponent: () => import('./features/accounts/accounts.component').then(m => m.AccountsComponent),
              children:[
                { path: 'connected-apps', loadComponent: () => import('./features/accounts/expenses/expenses.component').then(m => m.ExpensesComponent) },
                { path: 'transactions', loadComponent: () => import('./features/accounts/transaction/transaction.component').then(m => m.TransactionComponent) },
                { path: 'income', loadComponent: () => import('./features/accounts/income/income.component').then(m => m.IncomeComponent) },
                { path: 'invoice', loadComponent: () => import('./features/accounts/invoice/invoice.component').then(m => m.InvoiceComponent) },
                { path: 'add-invoice', loadComponent: () => import('./features/accounts/add-invoice/add-invoice.component').then(m => m.AddInvoiceComponent) },
                { path: 'expenses', loadComponent: () => import('./features/accounts/expenses/expenses.component').then(m => m.ExpensesComponent) },
                { path: 'expenses-category', loadComponent: () => import('./features/accounts/expenses-category/expenses-category.component').then(m => m.ExpensesCategoryComponent) },
                { path: 'edit-invoice', loadComponent: () => import('./features/accounts/edit-invoice/edit-invoice.component').then(m => m.EditInvoiceComponent) },
                { path: 'accounts-invoices', loadComponent: () => import('./features/accounts/accounts-invoices/accounts-invoices.component').then(m => m.AccountsInvoicesComponent) }
              ]
             },
             //Announcements
               { path: 'event', loadComponent: () => import('./features/announcements/events/events.component').then(m => m.EventsComponent) },
              { path: 'notice-board', loadComponent: () => import('./features/announcements/notice-board/notice-board.component').then(m => m.NoticeBoardComponent) },
              //Reports
              { path: 'attendance-report', loadComponent: () => import('./features/reports/attendance-report/attendance-report.component').then(m => m.AttendanceReportComponent) },
              { path: 'class-report', loadComponent: () => import('./features/reports/class-report/class-report.component').then(m => m.ClassReportComponent) },
              { path: 'daily-attendance', loadComponent: () => import('./features/reports/daily-attendance/daily-attendance.component').then(m => m.DailyAttendanceComponent) },
              { path: 'fees-report', loadComponent: () => import('./features/reports/fees-report/fees-report.component').then(m => m.FeesReportComponent) },
              { path: 'grade-report', loadComponent: () => import('./features/reports/grade-report/grade-report.component').then(m => m.GradeReportComponent) },
              { path: 'staff-day-wise', loadComponent: () => import('./features/reports/staff-day-wise/staff-day-wise.component').then(m => m.StaffDayWiseComponent) },
              { path: 'student-attendance-type', loadComponent: () => import('./features/reports/student-attendance-type/student-attendance-type.component').then(m => m.StudentAttendanceTypeComponent) },
              { path: 'staff-report', loadComponent: () => import('./features/reports/staff-report/staff-report.component').then(m => m.StaffReportComponent) },
              { path: 'leave-report', loadComponent: () => import('./features/reports/leave-report/leave-report.component').then(m => m.LeaveReportComponent) },
              { path: 'teacher-day-wise', loadComponent: () => import('./features/reports/teacher-day-wise/teacher-day-wise.component').then(m => m.TeacherDayWiseComponent) },
              { path: 'teacher-report', loadComponent: () => import('./features/reports/teacher-report/teacher-report.component').then(m => m.TeacherReportComponent) },
              { path: 'student-day-wise', loadComponent: () => import('./features/reports/student-day-wise/student-day-wise.component').then(m => m.StudentDayWiseComponent) },
              { path: 'student-report', loadComponent: () => import('./features/reports/student-report/student-report.component').then(m => m.StudentReportComponent) },
              //User Management
              { path: 'roles-permission', loadComponent: () => import('./features/user-management/roles-permission/roles-permission.component').then(m => m.RolesPermissionComponent) },
              { path: 'delete-account', loadComponent: () => import('./features/user-management/delete-account/delete-account.component').then(m => m.DeleteAccountComponent) },
              { path: 'users', loadComponent: () => import('./features/user-management/users/users.component').then(m => m.UsersComponent) },
              { path: 'permission', loadComponent: () => import('./features/user-management/permission/permission.component').then(m => m.PermissionComponent) },
              //Membership
              { path: 'membership-transactions', loadComponent: () => import('./features/membership/membership-transactions/membership-transactions.component').then(m => m.MembershipTransactionsComponent) },
              { path: 'membership-addons', loadComponent: () => import('./features/membership/membership-addons/membership-addons.component').then(m => m.MembershipAddonsComponent) },
              { path: 'membership-plans', loadComponent: () => import('./features/membership/membership-plans/membership-plans.component').then(m => m.MembershipPlansComponent) },
              //Content
              { path: 'blog', loadComponent: () => import('./features/content/blog/blog.component').then(m => m.BlogComponent),
                children: [
                    { path: 'blog-categories', loadComponent: () => import('./features/content/blog/blog-categories/blog-categories.component').then(m => m.BlogCategoriesComponent) },
                    { path: 'blog-comments', loadComponent: () => import('./features/content/blog/blog-comments/blog-comments.component').then(m => m.BlogCommentsComponent) },
                    { path: 'blog-tags', loadComponent: () => import('./features/content/blog/blog-tags/blog-tags.component').then(m => m.BlogTagsComponent) },
                    { path: 'all-blog', loadComponent: () => import('./features/content/blog/all-blog/all-blog.component').then(m => m.AllBlogComponent) }
                  ]
               },
              { path: 'location', loadComponent: () => import('./features/content/location/location.component').then(m => m.LocationComponent),
                children: [
                    { path: 'cities', loadComponent: () => import('./features/content/location/cities/cities.component').then(m => m.CitiesComponent) },
                    { path: 'countries', loadComponent: () => import('./features/content/location/countries/countries.component').then(m => m.CountriesComponent) },
                    { path: 'states', loadComponent: () => import('./features/content/location/states/states.component').then(m => m.StatesComponent) }
                  ]
               },
              { path: 'pages', loadComponent: () => import('./features/content/pages/pages.component').then(m => m.PagesComponent) },
              { path: 'faq', loadComponent: () => import('./features/content/faq/faq.component').then(m => m.FaqComponent) },
              { path: 'testimonials', loadComponent: () => import('./features/content/testimonials/testimonials.component').then(m => m.TestimonialsComponent) },
              //Support
               { path: 'contact-messages', loadComponent: () => import('./features/support/contact-messages/contact-messages.component').then(m => m.ContactMessagesComponent) },
                { path: 'tickets', loadComponent: () => import('./features/support/tickets/tickets.component').then(m => m.TicketsComponent),
                  children:[
                    { path: 'ticket-grid', loadComponent: () => import('./features/support/tickets/ticket-grid/ticket-grid.component').then(m => m.TicketGridComponent) },
                    { path: 'ticket-details', loadComponent: () => import('./features/support/tickets/ticket-details/ticket-details.component').then(m => m.TicketDetailsComponent) },
                    { path: 'ticket', loadComponent: () => import('./features/support/tickets/ticket/ticket.component').then(m => m.TicketComponent) },
                  ]
                 },
                 //Pages
                { path: 'profile', loadComponent: () => import('./features/pages/profile/profile.component').then(m => m.ProfileComponent) },
                { path: 'blank-page', loadComponent: () => import('./features/pages/blank-page/blank-page.component').then(m => m.BlankPageComponent) },
                { path: 'activities',loadComponent: () => import('./features/pages/activities/activities.component').then(m => m.ActivitiesComponent)},
                //Settings
                { path: 'academic-settings', loadComponent: () => import('./features/settings/academic-settings/academic-settings.component').then(m => m.AcademicSettingsComponent),
                  children:[
                    { path: 'school-settings', loadComponent: () => import('./features/settings/academic-settings/school-settings/school-settings.component').then(m => m.SchoolSettingsComponent) },
                    { path: 'religion', loadComponent: () => import('./features/settings/academic-settings/religion/religion.component').then(m => m.ReligionComponent) },
                  ]
                 },
                { path: 'app-settings', loadComponent: () => import('./features/settings/app-settings/app-settings.component').then(m => m.AppSettingsComponent),
                  children:[
                      { path: 'custom-fields', loadComponent: () => import('./features/settings/app-settings/custom-fields/custom-fields.component').then(m => m.CustomFieldsComponent) },
                      { path: 'invoice-settings', loadComponent: () => import('./features/settings/app-settings/invoice-settings/invoice-settings.component').then(m => m.InvoiceSettingsComponent) },
                  ]
                 },
                { path: 'financial-settings', loadComponent: () => import('./features/settings/financial-settings/financial-settings.component').then(m => m.FinancialSettingsComponent),
                  children:[
                      { path: 'payment-gateways', loadComponent: () => import('./features/settings/financial-settings/payment-gateways/payment-gateways.component').then(m => m.PaymentGatewaysComponent) },
                      { path: 'tax-rates', loadComponent: () => import('./features/settings/financial-settings/tax-rates/tax-rates.component').then(m => m.TaxRatesComponent) },
                  ]
                 },
             
                { path: 'general-settings', loadComponent: () => import('./features/settings/general-settings/general-settings.component').then(m => m.GeneralSettingsComponent),
                  children:[
                      { path: 'connected-apps', loadComponent: () => import('./features/settings/general-settings/connected-apps/connected-apps.component').then(m => m.ConnectedAppsComponent) },
                      { path: 'notification-settings', loadComponent: () => import('./features/settings/general-settings/notifications-settings/notifications-settings.component').then(m => m.NotificationsSettingsComponent) },
                      { path: 'profile-settings', loadComponent: () => import('./features/settings/general-settings/profile-settings/profile-settings.component').then(m => m.ProfileSettingsComponent) },
                      { path: 'security-settings', loadComponent: () => import('./features/settings/general-settings/security-settings/security-settings.component').then(m => m.SecuritySettingsComponent)}
                  ]
                 },
                 { path: 'other-settings', loadComponent: () => import('./features/settings/other-settings/other-settings.component').then(m => m.OtherSettingsComponent),
                  children:[
                      { path: 'ban-ip-address', loadComponent: () => import('./features/settings/other-settings/ban-ip-address/ban-ip-address.component').then(m => m.BanIpAddressComponent) },
                      { path: 'storage', loadComponent: () => import('./features/settings/other-settings/storage/storage.component').then(m => m.StorageComponent) },
                  ]
                 },
                 { path: 'system-settings', loadComponent: () => import('./features/settings/system-settings/system-settings.component').then(m => m.SystemSettingsComponent),
                  children:[
                    { path: 'email-settings', loadComponent: () => import('./features/settings/system-settings/email-settings/email-settings.component').then(m => m.EmailSettingsComponent) },
                    { path: 'email-templates', loadComponent: () => import('./features/settings/system-settings/email-templates/email-templates.component').then(m => m.EmailTemplatesComponent) },
                    { path: 'gdpr-cookies', loadComponent: () => import('./features/settings/system-settings/gdpr-cookies/gdpr-cookies.component').then(m => m.GdprCookiesComponent) },
                    { path: 'otp-settings', loadComponent: () => import('./features/settings/system-settings/otp-settings/otp-settings.component').then(m => m.OtpSettingsComponent) },
                    { path: 'sms-settings', loadComponent: () => import('./features/settings/system-settings/sms-settings/sms-settings.component').then(m => m.SmsSettingsComponent) },
                    
                  ]
                 },
                { path: 'website-settings', loadComponent: () => import('./features/settings/website-settings/website-settings.component').then(m => m.WebsiteSettingsComponent),
                  children:[
                    { path: 'preferences', loadComponent: () => import('./features/settings/website-settings/preferences/preferences.component').then(m => m.PreferencesComponent) },
                    { path: 'prefixes', loadComponent: () => import('./features/settings/website-settings/prefixes/prefixes.component').then(m => m.PrefixesComponent) },
                    { path: 'language-settings', loadComponent: () => import('./features/settings/website-settings/language-settings/language-settings.component').then(m => m.LanguageSettingsComponent) },
                    { path: 'localization', loadComponent: () => import('./features/settings/website-settings/localization/localization.component').then(m => m.LocalizationComponent) },
                    { path: 'company-settings', loadComponent: () => import('./features/settings/website-settings/company-settings/company-settings.component').then(m => m.CompanySettingsComponent) },
                    { path: 'social-authentication', loadComponent: () => import('./features/settings/website-settings/social-authentication/social-authentication.component').then(m => m.SocialAuthenticationComponent) },
                  ]
                 },
                 //layout
             { path: 'layout-mini', loadComponent: () => import('./features/dashboards/modal-dashboard/modal-dashboard.component').then((m) => m.ModalDashboardComponent), },
              { path: 'layout-default', loadComponent: () => import('./features/dashboards/modal-dashboard/modal-dashboard.component').then((m) => m.ModalDashboardComponent), },
              { path: 'layout-box', loadComponent: () => import('./features/dashboards/modal-dashboard/modal-dashboard.component').then((m) => m.ModalDashboardComponent), },
              { path: 'dashboard/layout-rtl', loadComponent: () => import('./features/dashboards/modal-dashboard/modal-dashboard.component').then((m) => m.ModalDashboardComponent), },
              { path: 'layout-dark', loadComponent: () => import('./features/dashboards/modal-dashboard/modal-dashboard.component').then((m) => m.ModalDashboardComponent), },


            
      ],
   
      },
      { path: 'coming-soon', loadComponent: () => import('./features/pages/coming-soon/coming-soon.component').then(m => m.ComingSoonComponent) },
      { path: 'under-maintenance', loadComponent: () => import('./features/pages/under-maintenance/under-maintenance.component').then(m => m.UnderMaintenanceComponent) }
      
      
]as const;
