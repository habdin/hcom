from django.urls import path

from . import views

app_name = "pharmacy"

urlpatterns = [
    # Class-based List View
    path("drug/", views.DrugListView.as_view(), name="drug-list"),
    # Class-based Search View
    path("drug/search/", views.DrugSearchListView.as_view(), name="drug-search"),
    # Class-based Create View
    path("drug/add/", views.DrugCreateView.as_view(), name="create-drug"),
    # Class-based Read View
    path("drug/<int:pk>/", views.DrugDetailView.as_view(), name="drug-add"),
    # Class-based Update View
    path("drug/edit/<int:pk>/", views.DrugUpdateView.as_view(), name="update-drug"),
    # Class-based Delete View
    path("drug/delete/<int:pk>/", views.DrugDeleteView.as_view(), name="delete-drug"),
    # Function View for DataTables
    path("drug/json/", views.LoadData, name="drug-json"),
    path("company/", views.CompanyListView.as_view(), name="company-list"),
]
