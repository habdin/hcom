from django.urls import path

from . import views

app_name = "dummy"

urlpatterns = [
    # Class-based List View
    path("", views.DummyListView.as_view(), name="dummy-list"),
    # Class-based Search View
    path("search/", views.DummySearchListView.as_view(), name="search-dummy"),
    # Class-based Create record View
    path("add/", views.DummyCreateView.as_view(), name="create-dummy"),
    # Class-based Read View
    path("<int:pk>/", views.DummyDetailView.as_view(), name="read-dummy"),
    # Class-based Update View
    path("edit/<int:pk>/", views.DummyUpdateView.as_view(), name="update-dummy"),
    # Class-based Delete View
    path("delete/<int:pk>/", views.DummyDeleteView.as_view(), name="delete-dummy"),
    # View for Datatables
    path("json/", views.LoadData, name="dummy-json"),
    # Function-based List view (GET)
    # path('', views.list_view, name="dummy-list"),
    # Function-based Detail View
    # path('<int:pk>/', views.read_entry, name="read-dummy"),
    # Function-based Create record View
    # path("add/", views.create_entry, name="create-dummy"),
    # Function-based Update View
    # path('edit/<int:pk>/', views.update_entry, name='update-dummy'),
    # Function-based Delete View
    # path('delete/<int:pk>/', views.delete_entry, name='delete-dummy'),
]
