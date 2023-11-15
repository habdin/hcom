from django.urls import path
from .views import (
    DrugListView,
)

app_name = 'pharmacy'

urlpatterns = [
    path('', DrugListView.as_view(), name='drug-list')
]
