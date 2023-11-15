from django.urls import path

from . import views


app_name = 'clinic'
urlpatterns = [
    path('', views.ClinicListView.as_view(), name='clinic-home'),
    path('<int:id>/', views.Clinic_Detail, name="clinic-detail"),
]
