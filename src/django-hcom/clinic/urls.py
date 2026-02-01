from django.urls import path

from . import views

app_name = "clinic"

urlpatterns = [
    path("", views.ClinicListView.as_view(), name="clinic-list"),
    path("search/", views.ClinicSearchListView.as_view(), name="clinic-search"),
    path("add/", views.ClinicCreateView.as_view(), name="create-clinic"),
    path("<int:pk>/", views.ClinicDetailView.as_view(), name="clinic-detail"),
    path("<int:pk>/summary/", views.ClinicSummaryView.as_view(), name="clinic-summary"),
    path("edit/<int:pk>/", views.ClinicUpdateView.as_view(), name="clinic-update"),
    path("json/", views.load_data, name="json"),
]
