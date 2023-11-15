from django.urls import path

from .views import (LoadData, UserCreateView, UserDeleteView, UserDetailView,
                    UserListView, UserUpdateView)

app_name = 'users'

urlpatterns = [
    path('', UserListView.as_view(), name='user-list'),
    path('user-list/', LoadData),
    path('add/', UserCreateView.as_view(), name='new-user'),
    path('<int:pk>/', UserDetailView.as_view(), name='user-detail'),
    path('edit/<int:pk>/', UserUpdateView.as_view(), name="edit-user"),
    path('delete/<int:pk>/', UserDeleteView.as_view(), name="delete-user"),
]
