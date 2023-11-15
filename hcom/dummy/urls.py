from django.urls import path

from . import views

app_name = "dummy"

urlpatterns = [
    # Function-based List view (GET)
    # path('', views.list_view, name="dummy-list"),
    # Class-based List View
    path("", views.DummyListView.as_view(), name="dummy-list"),
    path("search/", views.DummySearchListView.as_view(), name="search-dummy"),
    # Function-based Detail View
    # path('<int:pk>/', views.read_entry, name="read-dummy"),
    # Class-based Detail View
    path("<int:pk>/", views.DummyDetailView.as_view(), name="read-dummy"),
    # Function-based Add new record View
    # path("add-2/", views.create_entry, name="create-dummy-2"),
    # Class-based Edit View
    path("add/", views.DummyCreateView.as_view(), name="create-dummy"),
    # Function-based Update View
    # path('edit/<int:pk>/', views.update_entry, name='update-dummy'),
    # Class-based Update View
    path("edit/<int:pk>/", views.DummyUpdateView.as_view(), name="update-dummy"),
    # Function-based Delete View
    # path('delete/<int:pk>/', views.delete_entry, name='delete-dummy'),
    # Class-based Delete View
    path("delete/<int:pk>/", views.DummyDeleteView.as_view(), name="delete-dummy"),
    # View for Datatables
    path("dummy-list/", views.LoadData, name="dummy-json"),
]
