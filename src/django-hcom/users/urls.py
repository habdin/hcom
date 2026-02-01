from django.urls import path

from . import views

app_name = "users"

urlpatterns = [
    path("", views.UserListView.as_view(), name="user-list"),
    path("search/", views.UserSearchListView.as_view(), name="user-search"),
    path("json/", views.LoadData, name="user-json"),
    path("add/", views.UserCreateView.as_view(), name="new-user"),
    path("<int:pk>/", views.UserDetailView.as_view(), name="user-detail"),
    path("edit/<int:pk>/", views.UserUpdateView.as_view(), name="edit-user"),
    path("delete/<int:pk>/", views.UserDeleteView.as_view(), name="delete-user"),
]
